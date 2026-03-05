using UnityEngine;

using System.Collections.Generic;

namespace Assets._Achromatic.Scripts.Beatmap {

    public class Beatmap {

        // public members ######################################################
        public BeatmapMeta meta;
        public BeatmapData data;
        public Queue<BeatmapNote> notesQ;

        public Vector2 origin;
        public float currentBeatCount;
        public readonly float horizontalSpeedInMainPiece;

        // public methods  #####################################################
        public float CalcXFromBeat(float beatCount) {
            return origin.x + beatCount * meta.horizontalUnitsPerBeat;
        }

        public float CalcCurrentXFromBeat() {
            return CalcXFromBeat(currentBeatCount);
        }

        public float CalcBeatCount(BeatmapNote note) {
            return (note.Bar - 1) * meta.beatPerBar
                    + (note.Beat - 1)
                    + (note.Subbeat - 1) * beatsPerDivision;
        }

        /// <returns>realtime beat count based on Audio Source time,
        /// start on <c>0.0f</c></returns>
        public float BeatCount {
            get {
                return p.music.Time * beatPerSec - preludeOffsetAsBeat;
            }
        }

        // MonoBehavior Lifecycle  #############################################

        public void Update() {
            // update current beat count
            currentBeatCount = p.music.Time * beatsPerSecond;
        }

        // constructor  ########################################################
        public Beatmap(PieceScript piece, BeatmapMeta beatmapMeta) {
            p = piece;
            meta = beatmapMeta;

            if (meta == null) {
                Debug.LogError("Beatmap:\tmust assign Beatmap Meta in Piece");
                return;
            }
            if (meta.file == null) {
                Debug.LogError("Beatmap:\tmust assign Beatmap File in Beatmap Meta");
                return;
            }

            // load data  ------------------------------------------------------
            data = JsonUtility.FromJson<BeatmapData>(meta.file.text);
            if (data.notes.Length == 0) {
                Debug.LogError("Beatmap:\tbeatmap file contains no notes: "
                        + meta.file.name);
            }

            // init vars  ------------------------------------------------------
            beatPerSec = meta.tempo / 60.0f;
            preludeOffsetAsBeat = meta.preludeSeconds * beatPerSec;
            beatsPerDivision = 1 / meta.subdivisionPerBeat;

            var pos3 = piece.mainPartPath.EvaluatePosition(0, 0f);
            origin = new Vector2(pos3.x, pos3.y);

            beatsPerSecond = meta.tempo / 60f;
            horizontalSpeedInMainPiece =
                    meta.horizontalUnitsPerBeat * beatsPerSecond;


            // fill notesQ  ----------------------------------------------------
            notesQ = new();
            foreach (BeatmapData.JsonDataNote jsonNote in data.notes) {
                notesQ.Enqueue(new BeatmapNote(jsonNote));
            }
        }


        // private members  ####################################################
        private readonly float beatsPerSecond;
        private readonly float beatPerSec;
        private readonly float preludeOffsetAsBeat;
        private readonly float beatsPerDivision;

        // cached references
        private readonly PieceScript p;
    }
}

