using UnityEngine;

namespace Assets._Achromatic.Scripts.Pieces {

    public class Starter {

        // constructor  ########################################################
        public Starter(PieceScript piece) {
            p = piece;

            if (p.playerManager == null || p.playerManager.player == null) {
                Debug.LogWarning("playerManager/player is null");
            } else {
                p.playerManager.player.OnTriggerEnter += HandleOnTriggerEnter;
                p.playerManager.player.OnTriggerExit += HandleOnTriggerExit;
            }
        }

        // MonoBehavior Lifecycle  #############################################

        public void OnDisable() {
            if (p.playerManager == null || p.playerManager.player == null) {
                Debug.LogWarning("playerManager/player is null");
            } else {
                p.playerManager.player.OnTriggerEnter -= HandleOnTriggerEnter;
                p.playerManager.player.OnTriggerExit -= HandleOnTriggerExit;
            }
        }

        public void Update() {
            if (isVampPlaying) {
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
        private bool isVampPlaying = false;

        // cached references
        private readonly PieceScript p;

        // private methods  ####################################################

        /// <summary>
        /// start the prelude routine, then main music part
        /// </summary>
        private void StartPiece() {
            if (p.debugMusicStaringBar == 0) {
                p.music.StartPreludeThenMainPiece();
                p.playerManager.StartPrelude();

            } else {
                // start music mid point for debug purpose
                p.music.DebugStartMusic(p.debugMusicStaringBar);
                p.playerManager.StartMainPiece(p.debugMusicStaringBar);
            }

            p.isControllingPlayer = true;
            isVampPlaying = false;
        }

        // event handlers  =====================================================
        private void HandleOnTriggerEnter(string triggerTag) {
            if (triggerTag == VAMP_TRIGGER_TAG && !isVampPlaying) {
                isVampPlaying = true;
                p.music.StartVamp();

            } else if (triggerTag == PRELUDE_TRIGGER_TAG && isVampPlaying) {
                isVampPlaying = false;

                StartPiece();
            }
        }

        private void HandleOnTriggerExit(string triggerTag) {
            if (triggerTag == VAMP_TRIGGER_TAG && isVampPlaying) {
                isVampPlaying = false;
                p.music.StopVamp();
            }
        }
    }

}
