using System;


namespace Assets._Achromatic.Scripts.Beatmaps {

    /// <summary>
    /// a <b>Serializable</b> equivalent of <c>BeatmapData</c>
    ///
    /// to allow save/load as JSON
    /// </summary>
    [Serializable]
    public class BeatmapData {

        /// <remark>
        /// must be in order of appearances
        /// </remark>
        public Note[] notes;
    }
}