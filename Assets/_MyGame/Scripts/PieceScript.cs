using System;
using System.Collections.Generic;
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
    private BeatmapElementsPool elementsPool;

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
    private class BeatmapElementsPool: IDisposable {  // =======================

        // constants  **********************************************************
        /// <summary>
        /// name of <c>GameObject</c> shown in Hierarchy
        /// </summary>
        private const string GAME_OBJECT_NAME = "BeatmapElementsPoolRoot";

        /// <summary>
        /// folder which contains all elements Prefabs in Resources
        /// </summary>
        private const string PREFAB_FOLDER_PATH =
                "Prefabs/BeatmapElements/";

        /// <summary>
        /// element prefab names under "Prefabs/BeatmapElements/"
        /// </summary>
        private static readonly string[] ELEMENTS_NAMES =
                { "Barline", "BeatLine" };

        /// <summary>
        /// singleton collection of prefabs
        /// </summary>
        private static Dictionary<string, GameObject> prefabs;

        /// <summary>
        /// pools of all Prefab objects across pieces
        /// </summary>
        private static Dictionary<GameObject, Queue<GameObject>> pools;


        // private members  ****************************************************
        private readonly GameObject root;
        private readonly HashSet<GameObject> activeInstances;

        // constructor & destructor  *******************************************

        /// <summary>
        /// instantiate during <c>Awake()</c>
        /// </summary>
        /// <param name="parentTransform">
        /// transform which the BeatmapElementsPool will be placed under
        /// </param>
        public BeatmapElementsPool(Transform parentTransform) {
            activeInstances = new HashSet<GameObject>();

            // create pool root  -----------------------------------------------
            root = new GameObject(GAME_OBJECT_NAME);
            root.transform.SetParent(parentTransform, false);

            // load prefabs from Resources if non existent
            if (prefabs == null) {
                prefabs = new Dictionary<string, GameObject>();

                // load Prefabs by types
                for (int i = 0; i < ELEMENTS_NAMES.Length; i++) {
                    string key = ELEMENTS_NAMES[i];
                    string path = PREFAB_FOLDER_PATH + key;
                    GameObject prefab = Resources.Load<GameObject>(path);

                    if (prefab == null) {
                        Debug.LogError(
                "PieceScript: missing Prefab: Resources/" + path);
                        continue;
                    }

                    prefabs[key] = prefab;
                }
            }

            // prewarm  --------------------------------------------------------
            if (pools == null) {
                pools = new Dictionary<GameObject, Queue<GameObject>>();

                // per element type
                foreach (GameObject prefab in prefabs.Values) {
                    Queue<GameObject> q = new Queue<GameObject>();

                    for (int i = 0; i < 15; i++) {
                        // todo instead of set amount of 15 instances
                        GameObject go = GameObject.Instantiate(prefab);
                        go.SetActive(false);
                        go.transform.SetParent(root.transform, false);
                        q.Enqueue(go);
                    }

                    // add to pools
                    pools[prefab] = q;
                }
            }
        }

        ~BeatmapElementsPool() {
            Clear();
        }

        // public methods  *****************************************************

        public void Clear() {
            // TODO
        }

        // IDisposable Implementation  *****************************************
        public void Dispose() {
            Clear();
            GC.SuppressFinalize(this);
        }


        /* HACK
        // Prewarm Prefab Instances  ========================================
        // Spawn Instance  ==================================================
        // spawn from pool or instantiate new if pool empty
        public GameObject Spawn(GameObject prefab, Vector3 pos,
                                Quaternion rot, Transform parent = null) {
            if (prefab == null)
                return null;

            if (!pools.TryGetValue(prefab, out var q)) {
                q = new Queue<GameObject>();
                pools[prefab] = q;
            }

            GameObject instance;
            if (q.Count > 0) {
                instance = q.Dequeue();
                instance.transform.SetParent(parent, false);
                instance.transform.position = pos;
                instance.transform.rotation = rot;
                instance.SetActive(true);
            } else {
                instance = GameObject.Instantiate(prefab, pos, rot, parent);
            }

            activeInstances.Add(instance);
            return instance;
        }

        // Recycle Instance  =================================================
        // deactivate and return to its prefab queue, parent to pool root
        public void Recycle(GameObject instance) {
            if (instance == null)
                return;
            if (!activeInstances.Remove(instance)) {
                // Not tracked as active, still safe to recycle
            }

            // try find matching prefab key by comparing prefab name prefix
            // NOTE: store a mapping if prefab->instance link required
            instance.SetActive(false);
            instance.transform.SetParent(root.transform, false);

            // fallback: place into any queue for same prefab reference
            // attempt to find the queue whose prefab name matches
            foreach (var kv in pools) {
                if (kv.Key.name == instance.name.Replace("(Clone)", "").Trim()) {
                    kv.Value.Enqueue(instance);
                    return;
                }
            }

            // if no matching pool, create a general bucket for this instance
            if (!pools.TryGetValue(instance, out var newQ)) {
                newQ = new Queue<GameObject>();
                pools[instance] = newQ;
            }
            newQ.Enqueue(instance);
        }

        // Clear and Destroy All  ============================================
        public void Clear() {
            // destroy active instances first  --------------------------------
            foreach (var inst in activeInstances) {
                if (inst != null)
                    GameObject.Destroy(inst);
            }
            activeInstances.Clear();

            // destroy pooled objects and clear queues  -----------------------
            foreach (var kv in pools) {
                var q = kv.Value;
                while (q.Count > 0) {
                    var go = q.Dequeue();
                    if (go != null)
                        GameObject.Destroy(go);
                }
            }
            pools.Clear();

            // destroy root GameObject  -------------------------------------
            if (root != null)
                GameObject.Destroy(root);
        }


    */

    }


    /// <summary>
    /// handle awake of beatmap element prefabs
    /// </summary>
    private void AwakeBeatmap() {
        // load & set up beatmap
        if (beatmapFile == null) {
            Debug.LogWarning("PieceScript: must provide beatmapFile");
        }
        beatmap = JsonUtility.FromJson<PieceBeatmap>(beatmapFile.text);

        tempoDiv60 = beatmap.tempo / 60.0f;
        preludeOffsetAsBeat = beatmap.preludeLength * tempoDiv60;

        // load element prefabs
        elementsPool = new BeatmapElementsPool(
                GameControllerScript.Instance.transform);
    }

    /// <summary>
    /// handle update of beatmap element prefabs
    /// </summary>
    private void UpdateBeatmap() {
        // TODO
    }


    // Control Player  #########################################################

    /// <summary>
    /// handle update of player's control
    /// </summary>
    private void UpdatePlayer() {
        // update user horizontal position
        float x = transform.position.x
                + CalcCurrentBeatCount() * beatmap.beatSpeed;
        Vector2 newPosition = new(x, playerRB.position.y);
        playerRB.MovePosition(newPosition);
    }


    private float CalcCurrentBeatCount() {
        return audioSource.time * tempoDiv60 - preludeOffsetAsBeat;
    }

}


// FIXME map need to distinguish b/t purposes of dash vs jump, also allow different actions for the same action