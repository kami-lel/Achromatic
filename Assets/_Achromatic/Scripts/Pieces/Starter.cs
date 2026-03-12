using UnityEngine;

namespace Assets._Achromatic.Scripts.Pieces {

    public class Starter {

        // constructor  ########################################################
        public Starter(PieceScript piece) {
            p = piece;

            if (p.playerManager == null || p.playerManager.player == null) {
                Debug.LogWarning("Starter:\tplayerManager/player is null");
            } else {
                // HACK
                // p.playerManager.player.OnTriggerEnter += HandleOnTriggerEnter;
                // p.playerManager.player.OnTriggerExit += HandleOnTriggerExit;
            }
        }

        // MonoBehavior Lifecycle  #############################################

        public void OnDisable() {
            if (p.playerManager == null || p.playerManager.player == null) {
                Debug.LogWarning("Starter:\tplayerManager/player is null");
            } else {
                // HACK
                // p.playerManager.player.OnTriggerEnter -= HandleOnTriggerEnter;
                // p.playerManager.player.OnTriggerExit -= HandleOnTriggerExit;
            }
        }

        public void Update() {
            if (GCS.I.states == GameState.VAMP) {
                // update vamp volume
                float distance = Vector2.Distance(
                        p.playerManager.player.transform.position,
                        p.preludeStartOrigin);
                float volume = p.vampDistantVsVolume.Evaluate(distance);
                p.music.UpdateVampVolume(volume);
            }
        }

        // constants  ##########################################################
        private const string VAMP_TRIGGER_TAG = "StartVampTrigger";
        private const string PRELUDE_TRIGGER_TAG = "StartPreludeTrigger";

        // private members  ####################################################
        // cached references
        private readonly PieceScript p;

        // private methods  ####################################################

        /// <summary>
        /// start the prelude routine, then main music part
        /// </summary>
        private void StartPreludeThenMainPiece() {
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
        }

        // event handlers  =====================================================
        private void HandleOnTriggerEnter(string triggerTag) {
            if ((GCS.I.states & GameState.EXPLORE_CONTROL) != 0 &&
                    triggerTag == VAMP_TRIGGER_TAG) {

                GCS.I.states = GameState.VAMP;
                p.music.StartVamp();

            } else if (GCS.I.states == GameState.VAMP &&
                    triggerTag == PRELUDE_TRIGGER_TAG) {

                StartPreludeThenMainPiece();
            }
        }

        private void HandleOnTriggerExit(string triggerTag) {
            if (GCS.I.states == GameState.VAMP &&
                    triggerTag == VAMP_TRIGGER_TAG) {

                GCS.I.states = GameState.EXPLORE;
                p.music.StopVamp();
            }
        }
    }

}
