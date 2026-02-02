using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// todo allows & give feedback for smashing input during: empty or climax
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
    /// start this piece of music at bar <i>n</i>,
    /// default to <c>0.0f</c> for normal play
    /// </summary>
    [SerializeField]
    private float musicStaringBar = 0.0f;

    // constants  ##############################################################

    /// <summary>
    /// height of note on board
    /// </summary>
    private const float NOTES_HEIGHT = 2.5f; // fixme more dynamic?

    // public members  #########################################################
    public ScoreTracker scoreTracker;

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
    /// local dynamic copy used for render
    /// </summary>
    private Queue<BeatmapNote> notesRenderQ;

    private JudgeCriteria judgeCriteria;

    // input related
    private InputPressedActions pressedActions;

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
        AwakeJudge();

        scoreTracker = new(beatmap);
    }

    public void OnEnable() {
        // take over control of player
        playerScript.SetPlayTypeAsExplore(false);
        playerRB.MovePosition(origin);

        // start input management
        playerInput.onActionTriggered += OnActionTriggered;
        pressedActions = InputPressedActions.NONE;

        OnEnableBeatmap();

        // start the music
        if (musicStaringBar != 0.0f) {
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
        judgeCriteria.DetectPassByMiss(audioSource.time);
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
                pressedActions |= InputPressedActions.JUMP;
                break;
            case "Dash":
                pressedActions |= InputPressedActions.DASH;
                break;
            case "PowerJump":
                pressedActions |= InputPressedActions.POWER_JUMP;
                break;
            case "Trigger":
                Trigger();
                break;
            }
            break;

        case InputActionPhase.Canceled:
            switch (a.name) {
            case "Jump":
                pressedActions &= ~InputPressedActions.JUMP;
                break;
            case "Dash":
                pressedActions &= ~InputPressedActions.DASH;
                break;
            case "PowerJump":
                pressedActions &= ~InputPressedActions.POWER_JUMP;
                break;
            }
            break;
        }
    }

    private void Trigger() {
        JudgeResult judgeResult = judgeCriteria.Judge(
                audioSource.time, pressedActions);
        scoreTracker.Record(judgeResult);

        // control player  -----------------------------------------------------
        if ((pressedActions & InputPressedActions.JUMP) != 0) {
            PlayerJump();
        } else if ((pressedActions & InputPressedActions.DASH) != 0) {
            PlayerDash();
        }

        GameControllerScript.Instance.tmpUpdateText(judgeResult,
                scoreTracker.combo,
                scoreTracker.runningScore);
        // todo add audio for feedback
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
        // todo make note disappear / animation when hit
        // place beatLine  -----------------------------------------------------
        float renderBoundaryOnBeat = currentBeatCount
                + beatmap.BarlineRenderDistance * beatPerBar;
        while (renderBoundaryOnBeat - lastBeatLineOnBeat > 1.0f) {
            float placeOnBeat = lastBeatLineOnBeat + 1.0f;

            prefabPool.Spawn("BeatLine",
                    new Vector2(CalcXFromBeat(placeOnBeat), 0.0f));

            lastBeatLineOnBeat = placeOnBeat;
        }

        // place barline  ------------------------------------------------------
        renderBoundaryOnBeat = currentBeatCount + beatmap.BarlineRenderDistance;
        while (renderBoundaryOnBeat - lastBarlineOnBeat > beatPerBar) {
            float placeOnBeat = lastBarlineOnBeat + beatPerBar;

            prefabPool.Spawn("Barline",
                    new Vector2(CalcXFromBeat(placeOnBeat), 0.0f));

            lastBarlineOnBeat = placeOnBeat;
        }

        // fixme barline placement overlaps beat lines
        // bug 1st barline missing

        // render notes  -------------------------------------------------------
        renderBoundaryOnBeat = currentBeatCount + beatmap.NoteRenderDistance;

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
                CalcXFromBeat(currentBeatCount), origin.y);
        playerRB.MovePosition(newPosition);
    }


    private void PlayerJump() {
        SFXMangerScript.Instance.PlayJump();

        // TODO
    }

    private void PlayerDash() {
        SFXMangerScript.Instance.PlayDash();
        // TODO player dash
    }


    // Judging #################################################################
    private void AwakeJudge() {
        judgeCriteria = new(beatmap);
        judgeCriteria.onDetectPassByMiss += OnDetectPassByMiss;
    }

    void OnDetectPassByMiss(object sender, EventArgs e) {
        scoreTracker.Record(JudgeResult.LATE_MISS);
        // todo handle pass by miss
    }
    // helpers  ################################################################

    /// <returns>realtime beat count based on Audio Source time,
    /// start on <c>0.0f</c></returns>
    private float CalcRealtimeBeatCount() {
        return audioSource.time * tempoDiv60 - preludeOffsetAsBeat;
    }

    private float CalcXFromBeat(float beatCount) {
        return origin.x + beatCount * beatmap.BeatSpeed;
    }

}


[Flags]
public enum InputPressedActions {
    NONE = 0,
    JUMP = 1 << 0,
    DASH = 1 << 1,
    POWER_JUMP = 1 << 2,
}

// fixme map need to distinguish b/t purposes of dash vs jump, also allow different actions for the same action