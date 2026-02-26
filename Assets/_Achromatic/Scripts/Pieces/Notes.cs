using UnityEngine;

using System.Collections.Generic;

using Assets._Achromatic.Scripts.Beatmap;


namespace Assets._Achromatic.Scripts.Pieces {

    /// <summary>
    /// load beatmap data from Beatmap file,
    /// also provide beatmap/notes related helpers
    /// </summary>
    public class Notes {

        public Notes(TextAsset beatmapFile, Music music, BeatmapSetting beatmapSetting) {
            this.music = music;
            setting = beatmapSetting;

            // load data
            if (beatmapFile == null) {
                Debug.LogError("must provide beatmapFile");
            }

            data = JsonUtility.FromJson<BeatmapData>(beatmapFile.text);
            if (data.notes.Length == 0) {
                Debug.LogError("beatmap file contains no notes: "
                        + beatmapFile.name);
            }

            // fill notesQ
            notesQ = new();
            foreach (BeatmapData.JsonDataNote jsonNote
                    in data.notes) {
                notesQ.Enqueue(new BeatmapNote(jsonNote));
            }
            // init vars
            beatPerSec = setting.tempo / 60.0f;
            preludeOffsetAsBeat = setting.preludeBarCount * beatPerSec;
            beatsPerDivision = 1 / setting.subdivisionPerBeat;
        }

        public readonly BeatmapData data;
        public readonly BeatmapSetting setting;  // cached
        public Queue<BeatmapNote> notesQ;

        /// <returns>realtime beat count based on Audio Source time,
        /// start on <c>0.0f</c></returns>
        public float BeatCount {
            get {
                return music.Time * beatPerSec - preludeOffsetAsBeat;
            }
        }

        private readonly Music music;  // cached

        private readonly float beatPerSec;
        private readonly float preludeOffsetAsBeat;
        private readonly float beatsPerDivision;
    }

}