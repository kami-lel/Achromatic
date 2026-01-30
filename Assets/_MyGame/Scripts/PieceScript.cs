using System;
using UnityEngine;
using UnityEngine.InputSystem;

// bug prelude not functioning, currently only working w/ prelude = 0
// todo allows & give feedback for smashing input during empty sessions
// todo need to be **fast** for sense of velocity
// Todo score system
// todo allow smash for song climax

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

    // private members  ########################################################
    // references
    private AudioSource audioSource;
    private PlayerScript playerScript;
    private Rigidbody2D playerRB;
    private PlayerInput playerInput;
    private Vector2 origin;

    // beatmap related
    private PieceBeatmap beatmap;
    private float tempoDiv60;
    private float preludeOffsetAsBeat;

    // input related
    private PressedActions pressedActions;

    // MonoBehavior Lifecycle  #################################################

    /// <summary>
    /// initialize PieceScript
    /// </summary>
    public void Awake() {
        // link references
        origin = (Vector2) transform.position;
        // link player references
        GameObject player = GameControllerScript.GetPlayer();
        playerRB = player.GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<PlayerScript>();
        playerInput = player.GetComponent<PlayerInput>();

        // set up audio
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // load & set up beatmap
        if (beatmapFile == null) {
            Debug.LogWarning("PieceScript: must provide beatmapFile");
        }
        beatmap = JsonUtility.FromJson<PieceBeatmap>(beatmapFile.text);

        tempoDiv60 = beatmap.tempo / 60.0f;
        preludeOffsetAsBeat = beatmap.preludeLength * tempoDiv60;
    }

    public void OnEnable() {
        // take over control of player
        playerScript.SetPlayTypeAsExplore(false);
        playerRB.MovePosition(origin);

        // start input management
        playerInput.onActionTriggered += OnActionTriggered;
        pressedActions = PressedActions.NONE;

        // start the music
        audioSource.Play();
    }

    public void Update() {
        // update user horizontal position
        float x = transform.position.x
                + CalcCurrentBeatCount() * beatmap.beatSpeed;
        Vector2 newPosition = new(x, playerRB.position.y);
        playerRB.MovePosition(newPosition);

        // TODO dynamically place tiles
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
        // Todo control user
        Debug.Log(pressedActions);
    }

    // helper methods  #########################################################
    private float CalcCurrentBeatCount() {
        return audioSource.time * tempoDiv60 - preludeOffsetAsBeat;
    }

    // helper enum  ############################################################
    [Flags]
    private enum PressedActions {
        NONE = 0,
        JUMP = 1 << 0,
        DASH = 1 << 1,
        POWER_JUMP = 1 << 2,
    }

    // todo add barline
}


// FIXME map need to distinguish b/t purposes of dash vs jump, also allow different actions for the same action