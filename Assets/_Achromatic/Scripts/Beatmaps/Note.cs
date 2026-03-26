using System;


namespace Assets._Achromatic.Scripts.Beatmaps {

    /// <summary>
    /// represent a single note w/i beatmap
    /// </summary>
    [Serializable]
    public class Note {

        public int bar;
        public int beat;
        public int subbeat;

        public string type;
    }
}

