using System.Collections.Generic;
using UnityEngine;

// Fixme code refactorization & make monobehavior
// Bug missing beat lines
/*
namespace Assets._Achromatic.Scripts.Beatmaps {

    public class ElementsManager {

        [SerializeField]
        private Transform prefabs;
        // constants  ========================================================
        private const string PREFAB_FOLDER_PATH = "Prefabs/BeatmapElements/";
        private const float NOTES_HEIGHT = 1.5f;

        // constructor  ==========================================================
        public ElementsManager(PieceScript pieceScript, Transform rootTransform) {
            p = pieceScript;
            root = rootTransform;

            // create per-type pools  --------------------------------------------
            beatLinePool = new PrefabPool(16, PREFAB_FOLDER_PATH + "BeatLine", root);
            barlinePool = new PrefabPool(8, PREFAB_FOLDER_PATH + "Barline", root);
            jumpNotePool = new PrefabPool(8, PREFAB_FOLDER_PATH + "JumpNote", root);
            dashNotePool = new PrefabPool(8, PREFAB_FOLDER_PATH + "DashNote", root);

            // initialize last placed positions to current playback state
            float currentBeat = p.beatmap.currentBeatCount;
            lastBeatLineOnBeat = Mathf.Floor(currentBeat);  // start at current beat
                                                            // align to the most recent bar boundary
            int beatsPerBar = p.beatmap.meta.beatPerBar;
            lastBarlineOnBeat = Mathf.Floor(currentBeat / beatsPerBar)
                                * beatsPerBar;

            // prepare notes queue starting from currentBeat  ---------------------
            notesRenderQ = new Queue<BeatmapNote>();
            foreach (var n in p.beatmap.notesQ) {
                float noteBeat = p.beatmap.CalcBeatCount(n);
                if (noteBeat >= currentBeat)
                    notesRenderQ.Enqueue(n);
            }
        }

        // public methods  ===================================================
        public void Update() {
            if ((GCS.I.states & GameState.PIECE_CONTROl) == 0) {
                return;
            }


            // place beatLine  -----------------------------------------------
            float renderBoundaryOnBeat = p.beatmap.currentBeatCount
                    + p.beatmap.meta.barlineRenderDistance
                    * p.beatmap.meta.beatPerBar;

            while (renderBoundaryOnBeat - lastBeatLineOnBeat > 1.0f) {
                float placeOnBeat = lastBeatLineOnBeat + 1.0f;

                beatLinePool?.Spawn(p.beatmap.CalcXFromBeat(placeOnBeat),
                                    0.0f);

                lastBeatLineOnBeat = placeOnBeat;
            }

            // place barline  -----------------------------------------------
            renderBoundaryOnBeat = p.beatmap.currentBeatCount
                    + p.beatmap.meta.barlineRenderDistance;

            while (renderBoundaryOnBeat - lastBarlineOnBeat
                    > p.beatmap.meta.beatPerBar) {
                float placeOnBeat = lastBarlineOnBeat
                        + p.beatmap.meta.beatPerBar;

                barlinePool?.Spawn(p.beatmap.CalcXFromBeat(placeOnBeat),
                                   0.0f);

                lastBarlineOnBeat = placeOnBeat;
            }

            // render notes  ------------------------------------------------
            float noteRenderBoundary = p.beatmap.currentBeatCount
                    + p.beatmap.meta.noteRenderDistance;

            while (notesRenderQ.Count > 0) {
                var next = notesRenderQ.Peek();
                float noteOnBeat = p.beatmap.CalcBeatCount(next);

                if (noteOnBeat >= noteRenderBoundary)
                    break;

                BeatmapNote note = notesRenderQ.Dequeue();

                if (note.type == NoteType.JUMP) {
                    jumpNotePool?.Spawn(p.beatmap.CalcXFromBeat(noteOnBeat),
                                        NOTES_HEIGHT);
                } else if (note.type == NoteType.DASH) {
                    dashNotePool?.Spawn(p.beatmap.CalcXFromBeat(noteOnBeat),
                                        NOTES_HEIGHT);
                }
            }
        }

        // private members  ==================================================
        private Queue<BeatmapNote> notesRenderQ;
        private readonly Transform root;

        // per-element pools  ------------------------------------------------
        private readonly PrefabPool beatLinePool;
        private readonly PrefabPool barlinePool;
        private readonly PrefabPool jumpNotePool;
        private readonly PrefabPool dashNotePool;

        private float lastBeatLineOnBeat;
        private float lastBarlineOnBeat;
    }

}
*/