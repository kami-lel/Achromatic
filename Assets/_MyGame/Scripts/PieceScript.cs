using System;
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
    private float debugMusicStaringBar = 0.0f;

    [SerializeField]
    private Collider2D preludePlayCollider;

    // public members  #########################################################
    public ScoreTracker scoreTracker;


    // MonoBehavior Lifecycle  #################################################
    private void Awake() {
        origin = (Vector2)transform.position;
        beatmap = new(origin, beatmapFile);
        judgeCriteria = new(beatmap.beatmapData);

        PlayerAwake();
        AudioAwake();
    }

    private void Start() {
        judgeCriteria.onDetectPassByMiss += OnDetectPassByMiss;

        PlayerStart();
        InputStart();
    }

    private void Update() {
        // calculate current beat count
        beatmap.currentBeatCount = beatmap.CalcRealtimeBeatCount(audioSource);

        BeatmapUpdate();
        PlayerUpdate();
        judgeCriteria.DetectPassByMiss(audioSource.time);
    }

    void OnDisable() {
        judgeCriteria.onDetectPassByMiss -= OnDetectPassByMiss;

        PlayerOnDisable();
    }

    // private members  ########################################################
    private Vector2 origin;
    private JudgeCriteria judgeCriteria;


    // player  #################################################################

    private PlayerScript playerScript;
    private Rigidbody2D playerRB;
    private PlayerInput playerInput;

    private void PlayerAwake() {
        // link player references
        GameObject player = GameControllerScript.GetPlayer();
        playerRB = player.GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<PlayerScript>();
        playerInput = player.GetComponent<PlayerInput>();
    }

    /// <summary>
    /// handle update of player's control
    /// </summary>
    private void PlayerUpdate() {

        float y = 0f;
        // float y = origin.y + tmpJumpCurve.Evaluate(Time.time - tmpPlayerLastJump);

        // update user horizontal position
        Vector2 newPosition = new(beatmap.CalcCurrentXFromBeat(), y);
        playerRB.MovePosition(newPosition);
    }

    private void PlayerStart() {
        playerScript.SetExplorePlay(false);
        playerRB.MovePosition(origin);
    }

    private void PlayerOnDisable() {
        playerScript.SetExplorePlay(true);
        playerInput.onActionTriggered -= OnActionTriggered;

    }



    // audio  ##################################################################
    private AudioSource audioSource;

    private void AudioAwake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void AudioStart() {
        // TODO make it actually work with prelude
        // start the music
        if (debugMusicStaringBar != 0.0f) {
            // start music midpoint, for debug purpose
            audioSource.time = (debugMusicStaringBar - 1.0f)
                    * beatmap.beatPerBar
                    * (60.0f / beatmap.beatmapData.Tempo)
                    + beatmap.beatmapData.PreludeLength;
        }
        audioSource.Play();
        audioSource.SetScheduledEndTime(AudioSettings.dspTime + 140f);
    }


    // beatmap  ################################################################
    private Beatmap beatmap;

    private void BeatmapUpdate() {
    }


    // Input  ##################################################################

    private InputPressedActions pressedActions;

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
                InputTrigger();
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

    private void InputTrigger() {
        JudgeResult judgeResult = judgeCriteria.Judge(
                audioSource.time, pressedActions);
        scoreTracker.Record(judgeResult);

        // control player  -----------------------------------------------------
        if ((pressedActions & InputPressedActions.JUMP) != 0) {
            playerScript.AnimationJump();
        } else if ((pressedActions & InputPressedActions.DASH) != 0) {
            playerScript.AnimationDash();
        }

        GameControllerScript.Instance.tmpUpdateText(judgeResult,
                scoreTracker.combo,
                scoreTracker.runningScore);
        // todo add audio for feedback
    }

    private void InputStart() {
        playerInput.onActionTriggered += OnActionTriggered;
        pressedActions = InputPressedActions.NONE;
    }

    // Judging  ################################################################

    private void OnDetectPassByMiss(object sender, EventArgs e) {
        scoreTracker.Record(JudgeResult.LATE_MISS);
        // todo handle pass by miss
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