using System;


namespace Assets._Achromatic.Scripts.Beatmap {

    /// <summary>
    /// a <b>Serializable</b> equivalent of <c>BeatmapData</c>
    ///
    /// to allow save/load as JSON
    /// </summary>
    [Serializable]
    public class BeatmapJsonData {
        public int beatPerBar;
        public int beatSubdivision;
        public float beatSpeed;
        public float preludeLength;
        public float tempo;
        public float barlineRenderDistance;
        public float noteRenderDistance;

        /// <remark>
        /// must be in order of appearances
        /// </remark>
        public BeatmapJsonDataNote[] notes;

        [Serializable]
        public class BeatmapJsonDataNote {

            public int bar;
            public int beat;
            public int subbeat;

            /// <remark>
            /// must be: "jump", "dash"
            /// </remark>
            public string type;
        }

    }
}