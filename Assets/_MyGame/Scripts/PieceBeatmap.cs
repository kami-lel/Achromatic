using System;


[Serializable]
public class PieceBeatmap {
    // data fields  ------------------------------------------------------------
    public int beatPerBar;          // beats per bar
    public int beatSubdivision;     // No count of sub-beats per beat
    public float beatSpeed;         // x-axis movement per beat
    public float preludeLength;     // seconds before piece starts
    public float tempo;             // ie BPM
    public Note[] notes;

    // supporting structures  --------------------------------------------------
    [Serializable]
    public class Note {
        public int bar;         // bar count, starting at 1
        public int beat;        // beat count w/i bar, start at 1
        public int subbeat;     // beat division count, start at 1
        public string type;

        /// <summary></summary>
        /// <returns>type of note, but instead of <c>string</c>,
        /// convert it as <c>enum NoteType</c></returns>
        /// <exception cref="System.IO.InvalidDataException"></exception>
        public NoteType AsEnum() {
            return type switch {
                "jump" => NoteType.DASH,
                _ => throw new System.IO.InvalidDataException(
                        $"PieceBeatMap: unknown note type string: {type}")
            };
        }
    }

    [Flags]
    public enum NoteType {
        JUMP,
        DASH
    }
}