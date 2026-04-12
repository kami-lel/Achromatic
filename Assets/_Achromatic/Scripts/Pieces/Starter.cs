using Assets._Achromatic.Scripts.Beatmaps;
using Assets._Achromatic.Scripts.Players;
using Cinemachine;
using UnityEngine;

// FIXME dont show miss type indicator during prelude

namespace Assets._Achromatic.Scripts.Pieces {
    [RequireComponent(typeof(MusicManager))]
    [RequireComponent(typeof(PlayerManager))]
    [RequireComponent(typeof(Piece))]
    [RequireComponent(typeof(Beatmap))]
    public class Starter: MonoBehaviour {
        // Inspector Fields  ###################################################

        [SerializeField]
        private int debugMusicStartingBar = 0;

        [SerializeField]
        private float preludeTransitionSecond = 0.0f;

        [SerializeField]
        private AnimationCurve vampDistantVsVolume;

        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        // MonoBehavior Lifecycle  #############################################
        private void Awake() { // test inspector fields  -----------------------
            if (vampDistantVsVolume == null) {
                Debug.LogError("must assign: Vamp Distance Vs Volume", this);
            }
            if (virtualCamera == null) {
                Debug.LogError("must assign: Virtual Camera", this);
            }

            // caching reference of piece  -------------------------------------
            piece = GetComponent<Piece>();
            if (piece == null) {
                Debug.LogError("fail to get: Piece", this);
            }
            music = GetComponent<MusicManager>();
            if (music == null) {
                Debug.LogError("fail to get: MusicManager", this);
            }
            playerManager = GetComponent<PlayerManager>();
            if (playerManager == null) {
                Debug.LogError("fail to get: Player Manager", this);
            }
            beatmap = GetComponent<Beatmap>();
            if (beatmap == null) {
                Debug.LogError("fail to get: beatmap", this);
            }

            // caching references of player  -----------------------------------
            GameObject playerGO = GameController.I.FindMainPlayer();

            player = playerGO.GetComponent<Player>();
            playerTransform = playerGO.GetComponent<Transform>();
            if (playerTransform == null) {
                Debug.LogError("fail to get: playerTransform", this);
            }

            if (debugMusicStartingBar != 0) {
                Debug.LogWarning("Debug Music Starting Bar is non-zero", this);
            }

            // setting check  --------------------------------------------------
            if (preludeTransitionSecond <= 0.0f
                    || preludeTransitionSecond >= beatmap.PreludeSeconds) {

                Debug.LogWarning(
                    "invalid preludeTransitionSecond value:"
                        + preludeTransitionSecond, this
                );
            }
        }

        private void Start() {
            if (player != null) {
                player.OnTriggerEnter += HandleOnTriggerEnter;
                player.OnTriggerExit += HandleOnTriggerExit;
            } else {
                Debug.LogError("fail to subscribe", this);
            }
        }

        public void Update() {
            if (GCS.I.states == GameState.VAMP) {
                // update vamp volume
                float distance = Vector2.Distance(
                    playerTransform.position,
                    piece.preludeStartOrigin
                );
                float volume = vampDistantVsVolume.Evaluate(distance);
                music.UpdateVampVolume(volume);
            }
        }

        private void FixedUpdate() {
            if (GCS.I.states == GameState.PRELUDE) {
                preludeElapsedTime += Time.fixedDeltaTime;

                float nextX = CalcPreludeX(preludeElapsedTime);
                player.rb.MovePosition(new Vector2(nextX, beatmap.origin.y));
            }
        }

        private void OnDisable() {
            if (player != null) {
                player.OnTriggerEnter -= HandleOnTriggerEnter;
                player.OnTriggerExit -= HandleOnTriggerExit;
            }
        }

        // event handler  ######################################################

        private void HandleOnTriggerEnter(string triggerTag) {
            if (
                (GCS.I.states & GameState.EXPLORE_CONTROL) != 0
                && triggerTag == VAMP_TRIGGER_TAG
            ) {
                GCS.I.states = GameState.VAMP;
                music.StartVamp();
            } else if (
                  GCS.I.states == GameState.VAMP
                  && triggerTag == PRELUDE_TRIGGER_TAG
              ) {
                StartMusicPlay();
            }
        }

        private void HandleOnTriggerExit(string triggerTag) {
            if (
                GCS.I.states == GameState.VAMP
                && triggerTag == VAMP_TRIGGER_TAG
            ) {
                GCS.I.states = GameState.EXPLORE;
                music.StopVamp();
            }
        }

        // constants  ##########################################################
        private const string VAMP_TRIGGER_TAG = "StartVampTrigger";
        private const string PRELUDE_TRIGGER_TAG = "StartPreludeTrigger";

        // private members  ####################################################
        // Cached References
        private Player player;
        private PlayerManager playerManager;
        private MusicManager music;
        private Piece piece;
        private Transform playerTransform;
        private Beatmap beatmap;

        private float preludeElapsedTime;
        private float preludeCurveA;
        private float preludeCurveB;
        private float preludeCurveC;

        // private methods  ####################################################

        private void StartMusicPlay() {
            if (debugMusicStartingBar == 0) {
                StartPrelude();
            } else {
                // start music mid point for debug purpose
                music.DebugStartMusic(debugMusicStartingBar);
                playerManager.StartMainPiece(debugMusicStartingBar);
                GCS.I.states = GameState.MAIN_PIECE;
            }

            virtualCamera.Priority = 20;
        }

        private void StartPrelude() {
            GCS.I.states = GameState.PRELUDE;
            playerManager.StartPrelude();

            // set up prelude movement  ----------------------------------------
            preludeElapsedTime = 0.0f;

            // calc movement

        }

        private float CalcPreludeX(float t) {
            return 0.0f; // HACK
            return preludeCurveA * t * t + preludeCurveB * t + preludeCurveC;
        }
    }
}
