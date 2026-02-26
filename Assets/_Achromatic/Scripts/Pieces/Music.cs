using Assets._Achromatic.Scripts.Beatmap;
using UnityEngine;

namespace Assets._Achromatic.Scripts.Pieces {

    public class Music {

        public Music(AudioSource bgm, AudioSource prelude, AudioSource mainSong) {
            this.bgm = bgm;
            this.prelude = prelude;
            this.mainSong = mainSong;

            if (bgm == null) {
                Debug.LogWarning("BGM audio source not given");
            } else {
                bgm.playOnAwake = true;  // auto start BGM
            }

            if (prelude == null) {
                Debug.LogWarning("Prelude audio source not given");
            } else {

                prelude.playOnAwake = false;
            }

            if (mainSong == null) {
                Debug.LogError("must assign Main Song audio source");
            }
            mainSong.playOnAwake = false;
        }

        private AudioSource bgm;

        private AudioSource prelude;

        private AudioSource mainSong;

        public float Time {
            get {
                return bgm.time;
            }
        }

        // TODO TODO

        public void Update(float dist) {
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



    // public float CalcBeatCount(BeatmapSetting beatmapSetting, float beatsPerDivision) {
    //     return (jsonNote.bar - 1) * beatmapSetting.beatPerBar
    //             + (jsonNote.beat - 1)
    //             + (jsonNote.subbeat - 1) * beatsPerDivision;
    // }
}