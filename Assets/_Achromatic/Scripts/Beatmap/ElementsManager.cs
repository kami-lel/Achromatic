using System.Collections.Generic;
using UnityEngine;

// fixme code refactorization
namespace Assets._Achromatic.Scripts.Beatmap {

    public class ElementsManager {
        // constants  ========================================================
        private const string PREFAB_FOLDER_PATH = "Prefabs/BeatmapElements/";
        private const float NOTES_HEIGHT = 1.5f;

        // constructor  ======================================================
        public ElementsManager(PieceScript pieceScript, Transform rootTransform) {
            p = pieceScript;
            root = rootTransform;

            // create per-type pools  ----------------------------------------
            beatLinePool = new PrefabPool(30,
                    PREFAB_FOLDER_PATH + "BeatLine", root);
            barlinePool = new PrefabPool(10,
                    PREFAB_FOLDER_PATH + "Barline", root);
            jumpNotePool = new PrefabPool(10,
                    PREFAB_FOLDER_PATH + "JumpNote", root);
            dashNotePool = new PrefabPool(10,
                    PREFAB_FOLDER_PATH + "DashNote", root);

            lastBeatLineOnBeat = 0.0f;
            lastBarlineOnBeat = 0.0f;
            notesRenderQ = new Queue<BeatmapNote>(p.beatmap.notesQ);
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

                if (note.type == BeatmapNoteType.JUMP) {
                    jumpNotePool?.Spawn(p.beatmap.CalcXFromBeat(noteOnBeat),
                                        NOTES_HEIGHT);
                } else if (note.type == BeatmapNoteType.DASH) {
                    dashNotePool?.Spawn(p.beatmap.CalcXFromBeat(noteOnBeat),
                                        NOTES_HEIGHT);
                }
            }
        }

        // private members  ==================================================
        private readonly PieceScript p;
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