using System.Collections.Generic;
using UnityEngine;

// FIXME code refactorization & make monobehavior
// BUG missing beat lines
//  TODO make prefabs disappearing as feed back
namespace Assets._Achromatic.Scripts.Beatmaps {
    [DefaultExecutionOrder(0)]
    [RequireComponent(typeof(Beatmap))]
    public class ElementsManager: MonoBehaviour {

        // Inspector Fields  ###################################################

        [SerializeField]
        private Transform prefabs;

        [SerializeField]
        private float renderDistanceX = 10f;

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            // check inspect fields  -------------------------------------------
            if (prefabs == null) {
                Debug.LogError("must assign: Prefabs");
                return;
            }

            // caching reference of piece  -------------------------------------
            beatmap = GetComponent<Beatmap>();
            if (beatmap == null) {
                Debug.LogError("fail to get: Beatmap");
            }

            // init vars
            renderDistanceBeat = renderDistanceX;

            // create per-type pools  --------------------------------------------
            // action hints
            actionHintJump = new(8, PREFAB_FOLDER + "ActionHintJump", prefabs);
            actionHintAttack = new(8, PREFAB_FOLDER + "ActionHintAttack", prefabs);
            actionHintSquat = new(8, PREFAB_FOLDER + "ActionHintSquat", prefabs);
            // beatLinePool = new PrefabPool(16, PREFAB_FOLDER + "BeatLine", prefabs);
            // barlinePool = new PrefabPool(8, PREFAB_FOLDER + "Barline", prefabs);

            /* HACK
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

                    */
        }


        private void Update() {
            if ((GCS.I.states & GameState.PIECE_CONTROl) == 0) {
                return;
            }



            /*

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
                */

        }

        // constants  ##########################################################
        private const string PREFAB_FOLDER = "Prefabs/BeatmapElements/";

        // private members  ####################################################
        private float renderDistanceBeat;

        // per-element pools
        private PrefabPool actionHintJump;
        private PrefabPool actionHintAttack;
        private PrefabPool actionHintSquat;

        // Cached Reference
        private Beatmap beatmap;

    }
}

/* HACK rm
    public class ElementsManager {

        private const float NOTES_HEIGHT = 1.5f;


        // public methods  ===================================================
        public void Update() {
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