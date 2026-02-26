using UnityEngine;

using Assets._Achromatic.Scripts.Beatmap;


namespace Assets._Achromatic.Scripts.Pieces {

    /// <summary>
    /// load beatmap data from Beatmap file,
    /// also provide beatmap/notes related helpers
    /// </summary>
    public class Notes {

        public Notes(TextAsset beatmapFile, Music musicManager) {
            if (beatmapFile == null) {
                Debug.LogWarning("must provide beatmapFile");
            }

            // load data
            data = new(beatmapFile);

            // cache musicManger
            this.music = musicManager;

            tempoDiv60 = data.Tempo / 60.0f;
            preludeOffsetAsBeat = data.PreludeBarCount * tempoDiv60;
            beatsPerDivision = 1 / data.BeatSubdivision;
        }

        public readonly BeatmapData data;

        /// <returns>realtime beat count based on Audio Source time,
        /// start on <c>0.0f</c></returns>
        public float BeatCount {
            get {
                return music.Time * tempoDiv60 - preludeOffsetAsBeat;
            }
        }

        private readonly Music music;  // cached
        private readonly float tempoDiv60;
        private readonly float preludeOffsetAsBeat;
        private readonly float beatsPerDivision;

        // Hack
        // // fill notes
        // notes = new();
        // foreach (BeatmapJsonData.BeatmapJsonDataNote jsonNote
        //         in jsonData.notes) {
        //     notes.Enqueue(new BeatmapNote(this, jsonNote));
        // }

        // if (notes.Count == 0) {
        //     Debug.LogError("BeatmapData: beatmap file contains no notes: "
        //             + beatmapFile.name);
        // }
    }

}