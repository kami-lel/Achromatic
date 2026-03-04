using UnityEngine;

using System.Collections.Generic;

using Assets._Achromatic.Scripts.Beatmap;


namespace Assets._Achromatic.Scripts.Pieces {

    /// <summary>
    /// load beatmap data from Beatmap file,
    /// also provide beatmap/notes related helpers
    /// </summary>
    public class NotesManager {

        public NotesManager(MusicManager music, BeatmapMeta beatmapMeta) {
            // Fixme save piece as cached reference

            this.music = music;
            this.beatmapMeta = beatmapMeta;

            if (beatmapMeta == null) {
                Debug.LogError("must assign Beatmap Meta in Piece");
                return;
            }
            if (beatmapMeta.file == null) {
                Debug.LogError("must assign Beatmap File in Beatmap Meta");
                return;
            }

            beatmapData = JsonUtility.FromJson<BeatmapData>(beatmapMeta.file.text);
            if (beatmapData.notes.Length == 0) {
                Debug.LogError("beatmap file contains no notes: "
                        + beatmapMeta.file.name);
            }

            // fill notesQ
            notesQ = new();
            foreach (BeatmapData.JsonDataNote jsonNote
                    in beatmapData.notes) {
                notesQ.Enqueue(new BeatmapNote(jsonNote));
            }
            // init vars
            beatPerSec = this.beatmapMeta.tempo / 60.0f;
            preludeOffsetAsBeat = this.beatmapMeta.preludeSeconds * beatPerSec;
            beatsPerDivision = 1 / this.beatmapMeta.subdivisionPerBeat;
        }

        public readonly BeatmapData beatmapData;
        public readonly BeatmapMeta beatmapMeta;  // cached
        public Queue<BeatmapNote> notesQ;

        /// <returns>realtime beat count based on Audio Source time,
        /// start on <c>0.0f</c></returns>
        public float BeatCount {
            get {
                return music.Time * beatPerSec - preludeOffsetAsBeat;
            }
        }

        private readonly MusicManager music;  // cached

        private readonly float beatPerSec;
        private readonly float preludeOffsetAsBeat;
        private readonly float beatsPerDivision;
    }

}