using System;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;



// BeatmapData  ################################################################
/// <summary>
/// <b>data structure</b> represent a single piece of music's beatmap
/// </summary>
public class BeatmapData {

    public BeatmapJsonData jsonData;

    /// <summary>
    /// beats per bar
    /// </summary>
    public int BeatPerBar => jsonData.beatPerBar;

    /// <summary>
    /// number count of sub-beats per beat
    /// </summary>
    public int BeatSubdivision => jsonData.beatSubdivision;

    /// <summary>
    /// x-axis movement speed, unit per beat
    /// </summary>
    public float BeatSpeed => jsonData.beatSpeed;

    /// <summary>
    /// seconds before piece starts
    /// </summary>
    public float PreludeLength => jsonData.preludeLength;

    /// <summary>
    /// i.e. BPM
    /// </summary>
    public float Tempo => jsonData.tempo;

    public BeatmapNote[] notes;

    public BeatmapData(TextAsset beatmapFile) {
        jsonData = JsonUtility.FromJson<BeatmapJsonData>(beatmapFile.text);

        // fill notes
        notes = new BeatmapNote[jsonData.notes.Length];
        for (int i = 0; i < notes.Length; i++) {
            notes[i] = new BeatmapNote(jsonData.notes[i]);
        }
    }
}




// BeatmapNote  ################################################################
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

    private readonly BeatmapJsonData.BeatmapJsonDataNote jsonNote;

    public BeatmapNote(BeatmapJsonData.BeatmapJsonDataNote jsonNote) {
        this.jsonNote = jsonNote;

        // convert string to enum type
        type = jsonNote.type switch {
            "jump" => BeatmapNoteType.JUMP,
            "dash" => BeatmapNoteType.DASH,
            _ => throw new InvalidOperationException(
                $"BeatmapNote: bad note type: {jsonNote.type}")
        };
    }
}


// BeatmapData  ################################################################
/// <summary>
/// flags for a single element note type in beatmap
/// </summary>
[Flags]
public enum BeatmapNoteType {
    JUMP,
    DASH
}




// BeatmapJsonData  ############################################################
/// <summary>
/// a <b>Serializable</b> equivalent of <c>BeatmapData</c>
/// to allow save/load as JSON
/// </summary>
[Serializable]
public class BeatmapJsonData {
    public int beatPerBar;
    public int beatSubdivision;
    public float beatSpeed;
    public float preludeLength;
    public float tempo;

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
