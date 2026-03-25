using System.Collections.Generic;
using System.Linq;
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
            renderDistanceBeat = renderDistanceX;  // TODO

            // local copy queue
            distNotesQ = new Queue<(float, Note)>();
            float dist;
            foreach (Note note in beatmap.notesQ) {
                dist = 0f;  // TODO
                distNotesQ.Enqueue((dist, note));
            }
            if (distNotesQ.Count() == 0) {
                Debug.LogError("empty notesQ");
            }

            // create per-type pools  --------------------------------------------
            // action hints
            actionHintJumpPool = new(8, PREFAB_FOLDER + "ActionHintJump", prefabs);
            actionHintAttackPool = new(8, PREFAB_FOLDER + "ActionHintAttack", prefabs);
            actionHintSquatPool = new(8, PREFAB_FOLDER + "ActionHintSquat", prefabs);

            /* HACK
            // beatLinePool = new PrefabPool(16, PREFAB_FOLDER + "BeatLine", prefabs);
            // barlinePool = new PrefabPool(8, PREFAB_FOLDER + "Barline", prefabs);
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


            // place action hints  ---------------------------------------------




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

                */

        }

        private void OnDestroy() {
            actionHintJumpPool.Dispose();
            actionHintAttackPool.Dispose();
            actionHintSquatPool.Dispose();
        }

        // constants  ##########################################################
        private const string PREFAB_FOLDER = "Prefabs/BeatmapElements/";

        // private members  ####################################################
        private float renderDistanceBeat;
        private Queue<(float, Note)> distNotesQ;  // distance & notes queue

        // per-element pools
        private PrefabPool actionHintJumpPool;
        private PrefabPool actionHintAttackPool;
        private PrefabPool actionHintSquatPool;

        // Cached Reference
        private Beatmap beatmap;
    }
}
