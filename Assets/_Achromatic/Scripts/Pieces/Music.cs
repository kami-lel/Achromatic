using Assets._Achromatic.Scripts.Beatmap;
using UnityEngine;

namespace Assets._Achromatic.Scripts.Pieces {

    public class MusicManager {

        // public API  #########################################################

        public float Time {
            get {
                return pseudoAudioPlugin.mainPiece.time;
            }
        }

        public void Start() {
            pseudoAudioPlugin.StartMap();
        }

        // Constructor  ########################################################
        public MusicManager(PseudoAudioPlugin pseudoAudioPlugin, BeatmapMeta beatmapSetting) {
            this.pseudoAudioPlugin = pseudoAudioPlugin;
            this.beatmapSetting = beatmapSetting;
        }

        // private members  ####################################################
        // cached references
        private readonly PseudoAudioPlugin pseudoAudioPlugin;
        private readonly BeatmapMeta beatmapSetting;


        // Hack CalcBeatCount
        // public float CalcBeatCount(BeatmapSetting beatmapSetting, float beatsPerDivision) {
        //     return (jsonNote.bar - 1) * beatmapSetting.beatPerBar
        //             + (jsonNote.beat - 1)
        //             + (jsonNote.subbeat - 1) * beatsPerDivision;
        // }
    }
}