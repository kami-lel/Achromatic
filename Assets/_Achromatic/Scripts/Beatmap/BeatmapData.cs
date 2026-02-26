using System.Collections.Generic;

using UnityEngine;


namespace Assets._Achromatic.Scripts.Beatmap {

    /// <summary>
    /// <b>data structure</b> represent a single piece of music's beatmap
    /// </summary>
    public class BeatmapData {

        // public properties  ##################################################

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
        /// seconds before piece start,
        /// i.e. period of time before 1st beat of 1st bar
        /// </summary>
        public float PreludeLength => jsonData.preludeLength;

        /// <summary>
        /// i.e. BPM
        /// </summary>
        public float Tempo => jsonData.tempo;

        /// <summary>
        /// control render distance of barline and beat lines<br>
        ///
        /// in <b>count of bars</b>,
        /// how many bars of both barline & beat lines will be rendered
        /// in advance of the music
        /// </summary>
        public float BarlineRenderDistance => jsonData.barlineRenderDistance;

        /// <summary>
        /// control render distance of beatmap notes<br>
        /// in <b>count of beats</b>,
        /// how many notes will be rendered in advanced of the music
        /// </summary>
        public float NoteRenderDistance => jsonData.noteRenderDistance;

        /// <summary>
        /// 1 / beatSubdivision, pre-calculated for efficiency
        /// </summary>
        public float beatPerDivision;

        public Queue<BeatmapNote> notes;

        // private properties  #################################################
        private readonly BeatmapJsonData jsonData;

        // constructor  ########################################################
        public BeatmapData(TextAsset beatmapFile) {
            jsonData = JsonUtility.FromJson<BeatmapJsonData>(beatmapFile.text);
            beatPerDivision = 1 / jsonData.beatSubdivision;

            // fill notes
            notes = new();
            foreach (BeatmapJsonData.BeatmapJsonDataNote jsonNote
                    in jsonData.notes) {
                notes.Enqueue(new BeatmapNote(this, jsonNote));
            }

            if (notes.Count == 0) {
                Debug.LogError("BeatmapData: beatmap file contains no notes: "
                        + beatmapFile.name);
            }
        }
    }

}