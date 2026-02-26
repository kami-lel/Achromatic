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
    }


}