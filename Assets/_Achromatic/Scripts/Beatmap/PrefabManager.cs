
using System.Collections.Generic;

namespace Assets._Achromatic.Scripts.Beatmap {

    // Todo implement prefab manager
    public class PrefabManager {

        private Queue<BeatmapNote> notesRenderQ;

        public PrefabManager() {
            /*
            // load element prefabs
            prefabPool = new BeatmapPrefabsPool(
                    GameControllerScript.Instance.transform);

            lastBeatLineOnBeat = 0.0f;
            lastBarlineOnBeat = 0.0f;
            notesRenderQ = new(beatmapData.notes);
            */
        }

        public void Update() {
            /*

            // todo make note disappear / animation when hit
            // place beatLine  -----------------------------------------------------
            float renderBoundaryOnBeat = currentBeatCount
                    + beatmapData.BarlineRenderDistance * beatPerBar;
            while (renderBoundaryOnBeat - lastBeatLineOnBeat > 1.0f) {
                float placeOnBeat = lastBeatLineOnBeat + 1.0f;

                prefabPool.Spawn("BeatLine",
                        new Vector2(CalcXFromBeat(placeOnBeat), 0.0f));

                lastBeatLineOnBeat = placeOnBeat;
            }

            // place barline  ------------------------------------------------------
            renderBoundaryOnBeat = currentBeatCount + beatmapData.BarlineRenderDistance;
            while (renderBoundaryOnBeat - lastBarlineOnBeat > beatPerBar) {
                float placeOnBeat = lastBarlineOnBeat + beatPerBar;

                prefabPool.Spawn("Barline",
                        new Vector2(CalcXFromBeat(placeOnBeat), 0.0f));

                lastBarlineOnBeat = placeOnBeat;
            }

            // Fixme barline placement overlaps beat lines
            // Bug 1st barline missing

            // render notes  -------------------------------------------------------
            renderBoundaryOnBeat = currentBeatCount + beatmapData.NoteRenderDistance;

            while (notesRenderQ.Count > 0) {
                var next = notesRenderQ.Peek();
                float noteOnBeat = next.CalcBeatCount();

                if (noteOnBeat >= renderBoundaryOnBeat)
                    break;

                // place the note
                BeatmapNote note = notesRenderQ.Dequeue();

                string prefabName = note.type switch {
                    BeatmapNoteType.JUMP => "JumpNote",
                    BeatmapNoteType.DASH => "DashNote",
                    _ => null
                };

                prefabPool.Spawn(prefabName,
                        new Vector2(CalcXFromBeat(noteOnBeat), NOTES_HEIGHT));

            }
            */

        }


        // constants  ##########################################################
        private const float NOTES_HEIGHT = 1.5f; // Fixme more dynamic?

    }
}
