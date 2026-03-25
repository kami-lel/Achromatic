using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//  TODO make prefabs disappearing as feed back
namespace Assets._Achromatic.Scripts.Beatmaps {
    [DefaultExecutionOrder(0)]
    [RequireComponent(typeof(Beatmap))]
    public class ElementsManager: MonoBehaviour {

        // Inspector Fields  ###################################################

        [SerializeField]
        private Transform prefabs;

        public float renderDistanceBeat = 10f;

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

            lastBarlineOnBeat = 0.0f;
            lastBeatLineOnBeat = 0.0f;

            // local copy queue  -----------------------------------------------
            renderBeatNotesQ = new Queue<(float, Note)>();
            float beatCount;
            foreach (Note note in beatmap.notesQ) {
                beatCount = beatmap.CalcBeatCount(note);
                renderBeatNotesQ.Enqueue((beatCount, note));
            }
            if (renderBeatNotesQ.Count() == 0) {
                Debug.LogError("empty notesQ");
            }

            // create per-type pools  --------------------------------------------
            // action hints
            actionHintJumpPool = new(8, PREFAB_FOLDER + "ActionHintJump", prefabs);
            actionHintAttackPool = new(8, PREFAB_FOLDER + "ActionHintAttack", prefabs);
            actionHintSquatPool = new(8, PREFAB_FOLDER + "ActionHintSquat", prefabs);

            beatLinePool = new PrefabPool(16, PREFAB_FOLDER + "BeatLine", prefabs);
            barlinePool = new PrefabPool(4, PREFAB_FOLDER + "Barline", prefabs);
        }


        private void Update() {
            if ((GCS.I.states & GameState.PIECE_CONTROl) == 0) {
                return;
            }

            float currentRenderBeat = beatmap.currentBeat + renderDistanceBeat;

            // place beat line & barline  --------------------------------------
            while (lastBeatLineOnBeat < currentRenderBeat) {
            }

            // TODO TODO

            // TODO barlines

            // place action hints  ---------------------------------------------
            float onBeat;
            Note note;
            while (renderBeatNotesQ.Count > 0) {
                (onBeat, note) = renderBeatNotesQ.Peek();
                if (onBeat < currentRenderBeat) {
                    // TODO render

                    renderBeatNotesQ.Dequeue();

                } else {
                    break;
                }
            }



            /* FIXME

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
        // note beat, note object
        private Queue<(float, Note)> renderBeatNotesQ;
        private float lastBeatLineOnBeat;

        // per-element pools
        private PrefabPool actionHintJumpPool;
        private PrefabPool actionHintAttackPool;
        private PrefabPool actionHintSquatPool;
        private PrefabPool beatLinePool;
        private PrefabPool barlinePool;

        // Cached Reference
        private Beatmap beatmap;
    }
}
