using System;
using System.Collections.Generic;

using UnityEngine;


namespace Assets._Achromatic.Scripts.Beatmap {

    /// <summary>
    /// flags for a single element note type in beatmap
    /// </summary>
    [Flags]
    public enum BeatmapNoteType {
        JUMP,
        DASH
    }
}