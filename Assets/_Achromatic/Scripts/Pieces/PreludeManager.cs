using UnityEngine;

namespace Assets._Achromatic.Scripts.Pieces {

    public class PreludeManager {

        public PreludeManager(PieceScript piece, Transform vampLoudestOrigin, AnimationCurve vampDistantVsVolume) {
            this.piece = piece;
            this.vampLoudestOrigin = vampLoudestOrigin.position;
            this.vampDistantVsVolume = vampDistantVsVolume;


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
                float volume = vampDistantVsVolume.Evaluate(distance);
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
        private readonly AnimationCurve vampDistantVsVolume;


        // private methods  ####################################################
        private void HandleOnTriggerEnter(string triggerTag) {
            if (triggerTag == VAMP_TRIGGER_TAG && !isVampPlaying) {
                isVampPlaying = true;
                piece.music.StartVamp();

            } else if (triggerTag == PRELUDE_TRIGGER_TAG && isVampPlaying) {
                isVampPlaying = false;
                piece.music.StartPreludeThenMainPiece();
                // TODO TODO prelude logic
            }
        }

        private void HandleOnTriggerExit(string triggerTag) {
            if (triggerTag == VAMP_TRIGGER_TAG && isVampPlaying) {
                isVampPlaying = false;
                piece.music.StopVamp();
            }
        }
    }
}

// Hack

// private void AudioStart() {
//     // start music midpoint, for debug purpose
//     if (debugMusicStaringBar != 0.0f) {
//         audioSource.time = (debugMusicStaringBar - 1.0f)
//                 * beatmap.beatPerBar
//                 * (60.0f / beatmap.beatmapData.Tempo)
//                 + beatmap.beatmapData.PreludeLength;
//     }
//     // start the music
//     audioSource.Play();
//     audioSource.SetScheduledEndTime(AudioSettings.dspTime + 140f);
// }
//

// private void EnterPrelude() {
//     Debug.Log("PieceScript: player enters Prelude Play hit box");
//     phase = Phase.PRELUDE;
//     audioSource.Play();
// }

// private void LeavePrelude() {
//     phase = Phase.INIT;
//     audioSource.Stop();
// }
