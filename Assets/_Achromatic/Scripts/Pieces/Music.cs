using UnityEngine;

namespace Assets._Achromatic.Scripts.Pieces {

    public class Music {

        public Music(AudioSource audioSource) {
            this.audioSource = audioSource;
            audioSource.playOnAwake = false;
        }

        private AudioSource audioSource;

        public float Time {
            get {
                return audioSource.time;
            }
        }

        // TODO TODO

        public void Update(Phase phase, float dist) {
            // switch (phase) {
            // case Phase.PRELUDE:
            //     audioSource.volume = 1.0f - dist / 20f;
            //     if (audioSource.time > 8.0f) {
            //         audioSource.time = 0.0f;
            //     }
            //     break;

            // default:
            //     break;
            // }
        }

        // private void AudioStart() { // Hack
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

    }

}