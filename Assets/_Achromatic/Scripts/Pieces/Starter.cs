using UnityEngine;
using Assets._Achromatic.Scripts.Players;

namespace Assets._Achromatic.Scripts.Pieces {
    [RequireComponent(typeof(MusicManager))]
    public class Starter: MonoBehaviour {

        // Inspector Fields  ###################################################
        [SerializeField]
        private AnimationCurve vampDistantVsVolume;

        // MonoBehavior Lifecycle  #############################################
        private void Awake() {
            // test inspector fields  ------------------------------------------
            if (vampDistantVsVolume == null) {
                Debug.LogError("must assign: Vamp Distance Vs Volume");
            }

            // caching reference of piece  -------------------------------------
            music = GetComponent<MusicManager>();
            if (music == null) {
                Debug.LogError("fail to get: MusicManager");
            }

            // caching references of player  -----------------------------------
            GameObject playerGO = GCS.FindPlayer();

            player = playerGO.GetComponent<Player>();
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
            /* HACK
            if (GCS.I.states == GameState.VAMP) {
                // update vamp volume
                float distance = Vector2.Distance(
                        p.playerManager.player.transform.position,
                        p.preludeStartOrigin);
                float volume = p.vampDistantVsVolume.Evaluate(distance);
                p.music.UpdateVampVolume(volume);
            }
            */
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
        // Cached References
        private Player player;
        private MusicManager music;

        // private methods  ####################################################

        private void StartPreludeThenMainPiece() {
            /* HACK
            if (p.debugMusicStaringBar == 0) {
                p.music.StartPreludeThenMainPiece();
                p.playerManager.StartPrelude();
                GCS.I.states = GameState.PRELUDE;

            } else {
                // start music mid point for debug purpose
                p.music.DebugStartMusic(p.debugMusicStaringBar);
                p.playerManager.StartMainPiece(p.debugMusicStaringBar);
            }

            GCS.I.states = GameState.PRELUDE;
            p.virtualCamera.Priority = 20;
            */
        }
    }
}
