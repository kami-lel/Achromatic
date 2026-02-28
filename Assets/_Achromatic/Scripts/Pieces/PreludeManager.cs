using UnityEngine;

namespace Assets._Achromatic.Scripts.Pieces {

    public class PreludeManager {
        // public API  #########################################################

        public void Update() {

        }

        // Constructor  ########################################################
        public PreludeManager(MusicManager music, Collider2D startPreludeTrigger, Collider2D startMainPieceTrigger) {
            this.music = music;
            this.startPreludeTrigger = startPreludeTrigger;
            this.startMainPieceTrigger = startMainPieceTrigger;
        }

        // private members  ####################################################
        // cached references
        private readonly Collider2D startPreludeTrigger;
        private readonly Collider2D startMainPieceTrigger;
        private readonly MusicManager music;
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
