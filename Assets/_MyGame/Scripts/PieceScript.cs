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


    // constants  ##############################################################
    private const float RENDER_DIST_X = 10.0f;

    // private members  ########################################################
    // references
    private AudioSource audioSource;
    private PlayerScript playerScript;
    private Rigidbody2D playerRB;
    private PlayerInput playerInput;
    private Vector2 origin;

    // beatmap related
    private BeatmapData beatmap;
    private float tempoDiv60;
    private float preludeOffsetAsBeat;
    private BeatmapPrefabsPool prefabPool;
    private float lastBeatLineOnBeat;
    private float lastBarlineOnBeat;

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

        AwakeBeatmap();
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
        // Todo control user
        Debug.Log(pressedActions);
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
        tempoDiv60 = beatmap.Tempo / 60.0f;
        preludeOffsetAsBeat = beatmap.PreludeLength * tempoDiv60 - 1.0f;
        lastBeatLineOnBeat = 0.9f;
        lastBarlineOnBeat = 0.9f;
    }

    /// <summary>
    /// handle update of beatmap element prefabs
    /// </summary>
    private void UpdateBeatmap() {
        float beatCount = CalcCurrentBeatCount();

        // place beatLine  -----------------------------------------------------
        while (beatCount - lastBeatLineOnBeat >= 1.0f) {
            float placeOnBeat = (float) Math.Ceiling(lastBeatLineOnBeat);
            float placeOnX = (placeOnBeat - 1.0f) * beatmap.BeatSpeed
                    - origin.x;

            prefabPool.Spawn("beatLine", new Vector2(placeOnX, 0.0f));

            lastBarlineOnBeat = placeOnBeat;
            // BUG BUG BUG
        }



        // TODO barline & notes
    }


    // Control Player  #########################################################

    /// <summary>
    /// handle update of player's control
    /// </summary>
    private void UpdatePlayer() {
        // update user horizontal position
        float x = transform.position.x
                + CalcCurrentBeatCount() * beatmap.BeatSpeed;
        Vector2 newPosition = new(x, playerRB.position.y);
        playerRB.MovePosition(newPosition);
    }

    /// <returns>beat count, starting at 1</returns>
    private float CalcCurrentBeatCount() {
        return audioSource.time * tempoDiv60 - preludeOffsetAsBeat;
    }

}


// fixme map need to distinguish b/t purposes of dash vs jump, also allow different actions for the same action