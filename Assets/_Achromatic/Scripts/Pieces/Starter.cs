using UnityEngine;
using Cinemachine;
using Assets._Achromatic.Scripts.Players;

namespace Assets._Achromatic.Scripts.Pieces {
    [RequireComponent(typeof(MusicManager))]
    [RequireComponent(typeof(PlayerManager))]
    public class Starter: MonoBehaviour {

        // Inspector Fields  ###################################################

        [SerializeField]
        private int debugMusicStartingBar = 0;

        [SerializeField]
        private AnimationCurve vampDistantVsVolume;

        [SerializeField]
        private Transform startPreludeTransform;

        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        // MonoBehavior Lifecycle  #############################################
        private void Awake() {
            // test inspector fields  ------------------------------------------
            if (vampDistantVsVolume == null) {
                Debug.LogError("must assign: Vamp Distance Vs Volume");
            }
            if (startPreludeTransform == null) {
                Debug.LogError("must assign: Start Prelude Transform");
            }
            if (virtualCamera == null) {
                Debug.LogError("must assign: Virtual Camera");
            }

            // caching reference of piece  -------------------------------------
            music = GetComponent<MusicManager>();
            if (music == null) {
                Debug.LogError("fail to get: MusicManager");
            }
            playerManager = GetComponent<PlayerManager>();
            if (playerManager == null) {
                Debug.LogError("fail to get: Player Manager");
            }

            // caching references of player  -----------------------------------
            GameObject playerGO = GCS.FindPlayer();

            player = playerGO.GetComponent<Player>();
            playerTransform = playerGO.GetComponent<Transform>();
            if (playerTransform == null) {
                Debug.LogError("fail to get: playerTransform");
            }

            // caching references  ---------------------------------------------
            preludeStartOrigin = startPreludeTransform.position;
        }

        private void Start() {
            if (player != null) {
                player.OnTriggerEnter += HandleOnTriggerEnter;
                player.OnTriggerExit += HandleOnTriggerExit;
            } else {
                Debug.LogError("fail to subscribe");
            }
        }

        public void Update() {
            if (GCS.I.states == GameState.VAMP) {
                // update vamp volume
                float distance = Vector2.Distance(
                        playerTransform.position,
                        preludeStartOrigin);
                float volume = vampDistantVsVolume.Evaluate(distance);
                music.UpdateVampVolume(volume);
            }
        }


        private void OnDisable() {
            if (player != null) {
                player.OnTriggerEnter -= HandleOnTriggerEnter;
                player.OnTriggerExit -= HandleOnTriggerExit;
            } else {
                Debug.LogError("fail to unsubscribe");
            }
        }

        // event handler  ######################################################

        private void HandleOnTriggerEnter(string triggerTag) {
            if ((GCS.I.states & GameState.EXPLORE_CONTROL) != 0 &&
                    triggerTag == VAMP_TRIGGER_TAG) {

                GCS.I.states = GameState.VAMP;
                music.StartVamp();

            } else if (GCS.I.states == GameState.VAMP &&
                    triggerTag == PRELUDE_TRIGGER_TAG) {

                StartPreludeThenMainPiece();
            }
        }

        private void HandleOnTriggerExit(string triggerTag) {
            if (GCS.I.states == GameState.VAMP &&
                    triggerTag == VAMP_TRIGGER_TAG) {

                GCS.I.states = GameState.EXPLORE;
                music.StopVamp();
            }
        }

        // constants  ##########################################################
        private const string VAMP_TRIGGER_TAG = "StartVampTrigger";
        private const string PRELUDE_TRIGGER_TAG = "StartPreludeTrigger";


        // private members  ####################################################
        private Vector2 preludeStartOrigin;  // HACK

        // Cached References
        private Player player;
        private PlayerManager playerManager;
        private MusicManager music;
        private Transform playerTransform;

        // private methods  ####################################################

        private void StartPreludeThenMainPiece() {
            if (debugMusicStartingBar == 0) {
                music.StartPreludeThenMainPiece();
                playerManager.StartPrelude();
                GCS.I.states = GameState.PRELUDE;

            } else {
                // start music mid point for debug purpose
                music.DebugStartMusic(debugMusicStartingBar);
                playerManager.StartMainPiece(debugMusicStartingBar);
                GCS.I.states = GameState.MAIN_PIECE;
            }

            virtualCamera.Priority = 20;
        }
    }
}
