[System.Serializable]
public class PieceBeatmap {
    public int beatPerBar;          // beats per bar
    public int beatSubdivision;     // No count of sub-beats per beat
    public float beatSpeed;         // x-axis movement per beat
    public float preludeLength;     // seconds before piece starts
    public float tempo;             // ie BPM
    public PieceBeatmapNote[] notes;
}



[System.Serializable]
public class PieceBeatmapNote {
    public int bar;         // bar count, starting at 1
    public int beat;        // beat count w/i bar, start at 1
    public int subbeat;     // beat division count, start at 1
    public string type;
}
