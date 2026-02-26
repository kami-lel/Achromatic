using System;


namespace Assets._Achromatic.Scripts.Beatmap {

    /// <summary>
    /// represent a single note w/i beatmap
    /// </summary>
    public class BeatmapNote {

        /// <summary>
        /// bar count, starting at 1
        /// </summary>
        public int Bar => jsonNote.bar;

        /// <summary>
        /// beat count w/i bar, start at 1
        /// </summary>
        public int Beat => jsonNote.beat;

        /// <summary>
        /// beat division count, start at 1
        /// </summary>
        public int Subbeat => jsonNote.subbeat;

        public BeatmapNoteType type;

        private readonly BeatmapData.JsonDataNote jsonNote;

        public BeatmapNote(
                BeatmapData.JsonDataNote jsonNote) {

            this.jsonNote = jsonNote;

            // convert string to enum type
            type = jsonNote.type switch {
                "jump" => BeatmapNoteType.JUMP,
                "dash" => BeatmapNoteType.DASH,
                _ => throw new InvalidOperationException(
                    $"BeatmapNote: bad note type: {jsonNote.type}")
            };
        }

        public float CalcBeatCount(BeatmapSetting beatmapSetting, float beatsPerDivision) {
            return (jsonNote.bar - 1) * beatmapSetting.beatPerBar
                    + (jsonNote.beat - 1)
                    + (jsonNote.subbeat - 1) * beatsPerDivision;
        }
    }
}