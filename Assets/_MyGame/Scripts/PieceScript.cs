using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// todo allows & give feedback for smashing input during empty sessions
// todo need to be **fast** for sense of velocity
// Todo score system
// todo allow smash for song climax
// bug piece will have error if Active at beginning of scene

/// <summary>
/// controller during <c>Music Play</c>, enables:
/// <list type="bullet">
///   <item><description>
///     load and parse beatmap <c>.json</c> file
///   </description></item>
///   <item><description>
///     dynamically create and place <c>BeatmapElements</c> prefabs in scene
///   </description></item>
///   <item><description>
///   control player movement during play
///   </description></item>
/// </list>
/// </summary>
[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]
public class PieceScript: MonoBehaviour {

    // Inspector Fields  #######################################################
    [SerializeField]
    private TextAsset beatmapFile;

    /// <summary>
    /// should be <c>1.0f</c>, unless during debug
    /// </summary>
    [SerializeField]
    private float musicStaringBar = 1.0f;

    // Todo place some of these constants in json
    // constants  ##############################################################
    /// <summary>
    /// how many bars in advance that barline & beat lines will shown
    /// </summary>
    private const float BARLINE_RENDER_DISTANCE = 5.0f;

    /// <summary>
    /// height of note on board
    /// </summary>
    private const float NOTES_HEIGHT = 2.5f;

    /// <summary>
    /// how many beats before player, notes should render
    /// </summary>
    private const float NOTE_RENDER_DISTANCE = 12.0f;

    // private members  ########################################################
    // references
    private AudioSource audioSource;
    private PlayerScript playerScript;
    private Rigidbody2D playerRB;
    private PlayerInput playerInput;
    private Vector2 origin;

    // beatmap related
    private BeatmapData beatmap;
    private float beatPerBar;
    private float tempoDiv60;
    private float preludeOffsetAsBeat;
    private BeatmapPrefabsPool prefabPool;
    private float lastBeatLineOnBeat;
    private float lastBarlineOnBeat;

    /// <summary>
    /// current beat count, <c>0.0f</c> at start,
    /// consistent in the same <c>Update()</c>
    /// </summary>
    private float currentBeatCount;

    /// <summary>
    /// local dynamic copy used for render Q
    /// </summary>
    private Queue<BeatmapNote> notesRenderQ;

    // input related
    private PressedActions pressedActions;

    // MonoBehavior Lifecycle  #################################################

    /// <summary>
    /// initialize PieceScript
    /// </summary>
    public void Awake() {
        // link references
        origin = (Vector2)transform.position;
        // link player references
        GameObject player = GameControllerScript.GetPlayer();
        playerRB = player.GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<PlayerScript>();
        playerInput = player.GetComponent<PlayerInput>();

        // set up audio
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        AwakeBeatmap();
    }

    public void OnEnable() {
        // take over control of player
        playerScript.SetPlayTypeAsExplore(false);
        playerRB.MovePosition(origin);

        // start input management
        playerInput.onActionTriggered += OnActionTriggered;
        pressedActions = PressedActions.NONE;

        OnEnableBeatmap();

        // start the music
        if (musicStaringBar != 1.0f) {
            // start music midpoint, for debug purpose
            audioSource.time = (musicStaringBar - 1.0f)
                    * beatPerBar
                    * (60.0f / beatmap.Tempo)
                    + beatmap.PreludeLength;
        }
        audioSource.Play();

    }

    public void Update() {
        // calculate current beat count
        currentBeatCount = CalcRealtimeBeatCount();

        UpdateBeatmap();
        UpdatePlayer();
    }

    private void OnDisable() {
        // return control back to user
        playerScript.SetPlayTypeAsExplore(true);
        playerInput.onActionTriggered -= OnActionTriggered;
    }

    // input manage  ###########################################################
    private void OnActionTriggered(InputAction.CallbackContext ctxt) {
        InputAction a = ctxt.action;

        switch (a.phase) {
        case InputActionPhase.Started:
            switch (a.name) {
            case "Jump":
                pressedActions |= PressedActions.JUMP;
                break;
            case "Dash":
                pressedActions |= PressedActions.DASH;
                break;
            case "PowerJump":
                pressedActions |= PressedActions.POWER_JUMP;
                break;
            case "Trigger":
                Trigger();
                break;
            }
            break;

        case InputActionPhase.Canceled:
            switch (a.name) {
            case "Jump":
                pressedActions &= ~PressedActions.JUMP;
                break;
            case "Dash":
                pressedActions &= ~PressedActions.DASH;
                break;
            case "PowerJump":
                pressedActions &= ~PressedActions.POWER_JUMP;
                break;
            }
            break;
        }
    }

    private void Trigger() {
        // Todo user movement during music
        JudgeResult judgeResult = PerformJudge();

        // Todo visual & audio feedback for good/bad action
    }

    // helper enum  ============================================================
    [Flags]
    private enum PressedActions {
        NONE = 0,
        JUMP = 1 << 0,
        DASH = 1 << 1,
        POWER_JUMP = 1 << 2,
    }

    // Beatmap control #########################################################

    /// <summary>
    /// handle awake of beatmap element prefabs
    /// </summary>
    private void AwakeBeatmap() {
        // load & set up beatmap
        if (beatmapFile == null) {
            Debug.LogWarning("PieceScript: must provide beatmapFile");
        }

        beatmap = new BeatmapData(beatmapFile);

        // load element prefabs
        prefabPool = new BeatmapPrefabsPool(
                GameControllerScript.Instance.transform);

        // set up vars
        beatPerBar = beatmap.BeatPerBar;
        tempoDiv60 = beatmap.Tempo / 60.0f;
        preludeOffsetAsBeat = beatmap.PreludeLength * tempoDiv60;
    }

    /// <summary>
    /// handle OnEnable of beatmap element prefabs
    /// </summary>
    private void OnEnableBeatmap() {
        lastBeatLineOnBeat = 0.0f;
        lastBarlineOnBeat = 0.0f;
        notesRenderQ = new(beatmap.notes);
    }

    /// <summary>
    /// handle update of beatmap element prefabs
    /// </summary>
    private void UpdateBeatmap() {
        // place beatLine  -----------------------------------------------------
        float renderBoundaryOnBeat = currentBeatCount
                + BARLINE_RENDER_DISTANCE * beatPerBar;
        while (renderBoundaryOnBeat - lastBeatLineOnBeat > 1.0f) {
            float placeOnBeat = lastBeatLineOnBeat + 1.0f;

            prefabPool.Spawn("BeatLine",
                    new Vector2(CalcXFromBeat(placeOnBeat), 0.0f));

            lastBeatLineOnBeat = placeOnBeat;
        }

        // place barline  ------------------------------------------------------
        renderBoundaryOnBeat = currentBeatCount + BARLINE_RENDER_DISTANCE;
        while (renderBoundaryOnBeat - lastBarlineOnBeat > beatPerBar) {
            float placeOnBeat = lastBarlineOnBeat + beatPerBar;

            prefabPool.Spawn("Barline",
                    new Vector2(CalcXFromBeat(placeOnBeat), 0.0f));

            lastBarlineOnBeat = placeOnBeat;
        }

        // fixme barline placement overlaps beat lines
        // Bug 1st barline missing

        // render notes  -------------------------------------------------------
        renderBoundaryOnBeat = currentBeatCount + NOTE_RENDER_DISTANCE;

        while (notesRenderQ.Count > 0) {
            var next = notesRenderQ.Peek();
            float noteOnBeat = next.CalcBeatCount();

            if (noteOnBeat >= renderBoundaryOnBeat)
                break;

            // place the note
            BeatmapNote note = notesRenderQ.Dequeue();

            string prefabName = note.type switch {
                BeatmapNoteType.JUMP => "JumpNote",
                BeatmapNoteType.DASH => "DashNote",
                _ => null
            };

            prefabPool.Spawn(prefabName,
                    new Vector2(CalcXFromBeat(noteOnBeat), NOTES_HEIGHT));
        }
    }

    // Control Player  #########################################################

    /// <summary>
    /// handle update of player's control
    /// </summary>
    private void UpdatePlayer() {
        // update user horizontal position
        Vector2 newPosition = new(
                CalcXFromBeat(currentBeatCount), playerRB.position.y);
        playerRB.MovePosition(newPosition);
    }

    // Judge System  ###########################################################
    private enum JudgeResult {
        MISS, GOOD, GREAT, PERFECT
    }

    private JudgeResult PerformJudge() {
        // Todo implement judge system
        return JudgeResult.MISS;
    }


    // helpers  ################################################################

    /// <returns>beat count based on Audio Source time,
    /// start on <c>0.0f</c></returns>
    private float CalcRealtimeBeatCount() {
        return audioSource.time * tempoDiv60 - preludeOffsetAsBeat;
    }


    private float CalcXFromBeat(float beatCnt) {
        return origin.x + beatCnt * beatmap.BeatSpeed;
    }

}


// fixme map need to distinguish b/t purposes of dash vs jump, also allow different actions for the same action