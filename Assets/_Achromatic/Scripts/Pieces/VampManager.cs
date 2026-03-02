using UnityEngine;

namespace Assets._Achromatic.Scripts.Pieces {

    public class VampManager {

        public VampManager(PieceScript piece, Transform vampLoudestOrigin) {
            this.piece = piece;
            this.vampLoudestOrigin = vampLoudestOrigin.position;

            if (piece.playerManager == null || piece.playerManager.player == null) {
                Debug.LogWarning("playerManager/player is null");
            } else {
                piece.playerManager.player.OnTriggerEnter += HandleOnTriggerEnter;
                piece.playerManager.player.OnTriggerExit += HandleOnTriggerExit;
            }
        }

        public void OnDisable() {
            if (piece.playerManager == null || piece.playerManager.player == null) {
                Debug.LogWarning("playerManager/player is null");
            } else {
                piece.playerManager.player.OnTriggerEnter -= HandleOnTriggerEnter;
                piece.playerManager.player.OnTriggerExit -= HandleOnTriggerExit;
            }
        }

        public void Update() {
            if (isVampPlaying) {
                // update vamp volume
                float distance = Vector2.Distance(piece.playerManager.player.transform.position, vampLoudestOrigin);
                float volume = piece.vampDistantVsVolume.Evaluate(distance);
                piece.music.UpdateVampVolume(volume);
            }
        }

        // constants  ##########################################################
        private const string VAMP_TRIGGER_TAG = "StartVampTrigger";
        private const string PRELUDE_TRIGGER_TAG = "StartPreludeTrigger";

        // private members  ####################################################
        private bool isVampPlaying = false;

        // cached references
        private readonly PieceScript piece;
        private readonly Vector2 vampLoudestOrigin;

        // private methods  ####################################################
        private void HandleOnTriggerEnter(string triggerTag) {
            if (triggerTag == VAMP_TRIGGER_TAG && !isVampPlaying) {
                isVampPlaying = true;
                piece.music.StartVamp();

            } else if (triggerTag == PRELUDE_TRIGGER_TAG && isVampPlaying) {
                isVampPlaying = false;

                StartPreludeThenMainPiece();
            }
        }

        private void HandleOnTriggerExit(string triggerTag) {
            if (triggerTag == VAMP_TRIGGER_TAG && isVampPlaying) {
                isVampPlaying = false;
                piece.music.StopVamp();
            }
        }

        private void StartPreludeThenMainPiece() {
            if (piece.debugMusicStaringBar == 0) {
                piece.music.StartPreludeThenMainPiece();
                // TODO TODO prelude logic

            } else {
                // start music mid point for debug purpose
                piece.music.DebugStartMusic(piece.debugMusicStaringBar);
            }

            piece.playerManager.TakeOverPlayerControl();
        }
    }

}
