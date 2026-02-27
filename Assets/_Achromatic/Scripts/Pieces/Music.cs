using Assets._Achromatic.Scripts.Beatmap;
using UnityEngine;

namespace Assets._Achromatic.Scripts.Pieces {

    public class Music {

        // public API  #########################################################

        public float Time {
            get {
                return pseudoAudioPlugin.mainPiece.time;
            }
        }

        public Music(PseudoAudioPlugin pseudoAudioPlugin, BeatmapSetting beatmapSetting) {
            this.pseudoAudioPlugin = pseudoAudioPlugin;
            this.beatmapSetting = beatmapSetting;
        }

        // private members  ####################################################
        // cached references
        private readonly PseudoAudioPlugin pseudoAudioPlugin;
        private readonly BeatmapSetting beatmapSetting;

        // TODO

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

    }



    // public float CalcBeatCount(BeatmapSetting beatmapSetting, float beatsPerDivision) {
    //     return (jsonNote.bar - 1) * beatmapSetting.beatPerBar
    //             + (jsonNote.beat - 1)
    //             + (jsonNote.subbeat - 1) * beatsPerDivision;
    // }
}