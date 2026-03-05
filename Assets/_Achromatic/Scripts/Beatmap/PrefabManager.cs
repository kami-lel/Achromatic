
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Achromatic.Scripts.Beatmap {

    public class PrefabManager {

        private Queue<BeatmapNote> notesRenderQ;
        private PrefabsPool prefabsPool;
        private float lastBeatLineOnBeat;
        private float lastBarlineOnBeat;

        private readonly PieceScript p;

        public PrefabManager(PieceScript piece) {
            p = piece;

            // load element prefabs
            prefabsPool = new PrefabsPool(
                    GCS.I.transform);

            lastBeatLineOnBeat = 0.0f;
            lastBarlineOnBeat = 0.0f;
            notesRenderQ = new(p.beatmap.notesQ);
        }

        public void Update() {
            if (GCS.I.states != GameState.MAIN_PIECE) {
                return;
            }

            // todo make note disappear / animation when hit
            // place beatLine  -----------------------------------------------------
            float renderBoundaryOnBeat = p.beatmap.currentBeatCount
                    + p.beatmap.meta.barlineRenderDistance * p.beatmap.meta.beatPerBar;
            while (renderBoundaryOnBeat - lastBeatLineOnBeat > 1.0f) {
                float placeOnBeat = lastBeatLineOnBeat + 1.0f;

                prefabsPool.Spawn("BeatLine",
                        new Vector2(p.beatmap.CalcXFromBeat(placeOnBeat), 0.0f));

                lastBeatLineOnBeat = placeOnBeat;
            }


            // place barline  ------------------------------------------------------
            renderBoundaryOnBeat =
                    p.beatmap.currentBeatCount +
                    p.beatmap.meta.barlineRenderDistance;

            while (renderBoundaryOnBeat - lastBarlineOnBeat >
                    p.beatmap.meta.beatPerBar) {
                float placeOnBeat =
                        lastBarlineOnBeat + p.beatmap.meta.beatPerBar;

                prefabsPool.Spawn("Barline",
                        new Vector2(p.beatmap.CalcXFromBeat(placeOnBeat), 0.0f));

                lastBarlineOnBeat = placeOnBeat;
            }

            // FIXME barline placement overlaps beat lines
            // BUG 1st barline missing

            // render notes  -------------------------------------------------------
            renderBoundaryOnBeat =
                    p.beatmap.currentBeatCount +
                    p.beatmap.meta.noteRenderDistance;

            while (notesRenderQ.Count > 0) {
                var next = notesRenderQ.Peek();
                float noteOnBeat = p.beatmap.CalcBeatCount(next);

                if (noteOnBeat >= renderBoundaryOnBeat)
                    break;

                // place the note
                BeatmapNote note = notesRenderQ.Dequeue();

                string prefabName = note.type switch {
                    BeatmapNoteType.JUMP => "JumpNote",
                    BeatmapNoteType.DASH => "DashNote",
                    _ => null
                };

                prefabsPool.Spawn(prefabName,
                        new Vector2(
                                p.beatmap.CalcXFromBeat(noteOnBeat),
                                NOTES_HEIGHT));

            }

        }

        // constants  ##########################################################
        private const float NOTES_HEIGHT = 1.5f; // fixme more dynamic?

    }
}
