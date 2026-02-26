
using System;
using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

using Assets._Achromatic.Scripts.Scores;
using Assets._Achromatic.Scripts.Pieces;


// todo allows & give feedback for smashing input during: empty or climax
// todo background music during explore play
// Bug piece will have error if Active at beginning of scene


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

    [Header("Triggers")]

    [SerializeField]
    private Collider2D playerCollider;

    [SerializeField]
    private Collider2D playStartHitBox;


    // private members  ########################################################

    // managers
    private Music music;
    private Notes notes;

    // scores
    private Criteria criteria;
    private ScoreTracker scoreTracker;

    // cached references



    // MonoBehavior Lifecycle  #################################################
    private void Start() {
        phase = Phase.INIT;

        music = new(GetComponent<AudioSource>());
        notes = new(beatmapFile, music);

        criteria = new(notes.data);
        scoreTracker = new(notes.data);

        // HACK HACK
        PlayerStart();


        origin = (Vector2)transform.position;

        judgeCriteria = new(beatmap.beatmapData);
        judgeCriteria.onDetectPassByMiss += OnDetectPassByMiss;

        InputStart();
    }

    private void Update() {
        criteria.Update(phase);
        return;  // HACK

        // Hack use dist
        float dist = Vector2.Distance(playerCollider.transform.position, playStartHitBox.transform.position);
        switch (phase) {
        case Phase.INIT:
            if (dist <= 20.0f) {
                EnterPrelude();
            }
            break;

        case Phase.PRELUDE:
            if (playStartHitBox.IsTouching(playerCollider)) {
                StartPlay();
            } else if (dist > 20.0f) {
                LeavePrelude();
            }
            break;


        default:
            break;
        }

        switch (phase) {
        case Phase.PRELUDE:
            audioSource.volume = 1.0f - dist / 20f;
            if (audioSource.time > 8.0f) {
                audioSource.time = 0.0f;
            }
            break;

        default:
            break;
        }


        if (phase == Phase.MAIN_PLAY) {
            criteria.DetectPassByMiss();

            // calculate current beat count
            beatmap.currentBeatCount = beatmap.CalcRealtimeBeatCount(audioSource);

            BeatmapUpdate();
            PlayerUpdate();
            judgeCriteria.DetectPassByMiss();
            beatmap.Update();
        }
    }

    void OnDisable() {
        PlayerOnDisable();
    }

    // private members  ########################################################
    private Vector2 origin;
    private Criteria judgeCriteria;
    private Phase phase = Phase.INIT;

    private void EnterPrelude() {
        Debug.Log("PieceScript: player enters Prelude Play hit box");
        phase = Phase.PRELUDE;
        audioSource.Play();
    }

    private void LeavePrelude() {
        phase = Phase.INIT;
        audioSource.Stop();
    }

    private void StartPlay() {
        Debug.Log("PieceScript: player enters Start Play hit box");
        phase = Phase.MAIN_PLAY;

        playerScript.SetExplorePlay(false);
        playerInput.SwitchCurrentActionMap("PlayerMusicPlay");
        playerRB.MovePosition(origin);
        playerScript.AnimationStartWalk();


        if (_timerRoutine != null)
            StopCoroutine(_timerRoutine);  // stop old
        _timerRoutine = StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine() {
        double targetDsp = AudioSettings.dspTime + TARGET_SECONDS;  // compute dsp target
        while (AudioSettings.dspTime < targetDsp) {
            yield return null;  // wait until dspTime reaches target
        }
        _timerRoutine = null;  // clear handle
        SceneManager.LoadScene("EndScene");  // perform scene change
    }

    // player  #################################################################

    private PlayerScript playerScript;
    private Rigidbody2D playerRB;
    private PlayerInput playerInput;

    /// <summary>
    /// handle update of player's control
    /// </summary>
    private void PlayerUpdate() {

        float y = -0.8345073f; // Hack
        // float y = origin.y + tmpJumpCurve.Evaluate(Time.time - tmpPlayerLastJump);

        // update user horizontal position
        Vector2 newPosition = new(beatmap.CalcCurrentXFromBeat(), y);
        playerRB.MovePosition(newPosition);
    }

    private void PlayerStart() {
        // link player references
        GameObject player = GameControllerScript.GetPlayer();
        playerRB = player.GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<PlayerScript>();
        playerInput = player.GetComponent<PlayerInput>();

    }

    private void PlayerOnDisable() {
        if (playerScript != null) {
            playerScript.SetExplorePlay(true);
        }
        playerInput.onActionTriggered -= OnActionTriggered;

    }

    private const float TARGET_SECONDS = 169f;
    private Coroutine _timerRoutine;


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
        Hit judgeResult = judgeCriteria.Judge(
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
        // Todo add audio for feedback
    }

    private void InputStart() {
        playerInput.onActionTriggered += OnActionTriggered;
        pressedActions = InputPressedActions.NONE;
    }

    // Judging  ################################################################

    private void OnDetectPassByMiss(object sender, EventArgs e) {
        scoreTracker.Record(Hit.LATE_MISS);
        // Todo handle pass by miss
    }


}
[Flags]
public enum InputPressedActions {
    NONE = 0,
    JUMP = 1 << 0,
    DASH = 1 << 1,
    POWER_JUMP = 1 << 2,
}


// Bug audio start is jarring, lose framerate
// Fixme map need to distinguish b/t purposes of dash vs jump, also allow different actions for the same action