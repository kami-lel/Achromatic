using System;


/// <summary>
/// represent a single piece of music's beatmap
/// </summary>
[Serializable]
public class PieceBeatmap {
    // data fields  ------------------------------------------------------------
    /// <summary>
    /// beats per bar
    /// </summary>
    public int beatPerBar;

    /// <summary>
    /// number count of sub-beats per beat
    /// </summary>
    public int beatSubdivision;

    /// <summary>
    /// x-axis movement per beat
    /// </summary>
    public float beatSpeed;

    /// <summary>
    /// seconds before piece starts
    /// </summary>
    public float preludeLength;

    /// <summary>
    /// i.e. BPM
    /// </summary>
    public float tempo;

    public Note[] notes;

    // supporting structures  --------------------------------------------------
    /// <summary>
    /// represent a single note in beatmap
    /// </summary>
    [Serializable]
    public class Note {

        /// <summary>
        /// bar count, starting at 1
        /// </summary>
        public int bar;         //

        /// <summary>
        /// beat count w/i bar, start at 1
        /// </summary>
        public int beat;

        /// <summary>
        /// beat division count, start at 1
        /// </summary>
        public int subbeat;

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
