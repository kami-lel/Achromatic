using UnityEngine;

namespace Assets._Achromatic.Scripts.Pieces {

    public class PreludeManager {
        public PreludeManager(PieceScript piece) {
            this.piece = piece;
            pieceOrigin = piece.GetComponent<Transform>().position;

            if (piece.playerManager != null && piece.playerManager.player != null) {
                piece.playerManager.player.OnTriggerEnter += HandleTrigger;
            } else {
                Debug.LogWarning("fail to subscribe player OnTriggerEnter");
            }
        }

        public void OnDisable() {
            if (piece.playerManager != null && piece.playerManager.player != null) {
                piece.playerManager.player.OnTriggerEnter -= HandleTrigger;
            } else {
                Debug.LogWarning("fail to unsubscribe player OnTriggerEnter");
            }
        }


        // constants  ##########################################################
        private const string PLAYER_TAG = "Player";

        // private members  ####################################################
        // cached references
        private readonly PieceScript piece;
        private readonly Vector2 pieceOrigin;

        // private methods  ####################################################
        private void HandleTrigger(string triggerTag) {
            Debug.Log(triggerTag);  // HACK HACK

        }
    }
}

// TODO

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


// float dist = Vector2.Distance(playerCollider.transform.position, playStartHitBox.transform.position);
// // fixme fixed triggering distance
// // TODO TODO
// switch (GameControllerScript.Instance.gameState) {
// case GameState.EXPLORE:
//     if (dist <= 20.0f) {
//         Debug.Log("Start Prelude");
//         GameControllerScript.Instance.gameState |= GameState.PRELUDE;
//     }
//     break;

// case GameState.PRELUDE:
//     if (playStartHitBox.IsTouching(playerCollider)) {
//         playerManager.StartControlPlayer();

//     } else if (dist > 20.0f) {
//         GameControllerScript.Instance.gameState &= ~GameState.PRELUDE;
//     }
//     break;

// default:
//     break;
// }
