using System.Collections.Generic;
using System.Linq;
using Assets._Achromatic.Scripts.Scores;
using UnityEngine;

namespace Assets._Achromatic.Scripts.Beatmaps {
    [DefaultExecutionOrder(0)]
    [RequireComponent(typeof(Beatmap))]
    public class ElementsManager: MonoBehaviour {

        // Public Methods  #####################################################

        public void PerishActionHint(int noteIdx, Hit hit) {
            void SearchActiveQInPool(PrefabPool<int> prefabPool, out GameObject go) {
                var enumerator = prefabPool.activeQ.GetEnumerator();
                while (enumerator.MoveNext()) {
                    (int i, GameObject g) = enumerator.Current;
                    if (i == noteIdx) {
                        go = g;
                        return;
                    }
                }
                go = null;
            }

            // routine  ********************************************************

            if (noteIdx == -1) {
                return;
            }

            GameObject go;
            SearchActiveQInPool(actionHintJumpPool, out go);
            SearchActiveQInPool(actionHintAttackPool, out go);
            SearchActiveQInPool(actionHintSquatPool, out go);

            if (go == null) {
                Debug.LogWarning("fail to perish Action Hint w/ index of: " + noteIdx);
                return;
            }

            ActionHint hint = go.GetComponent<ActionHint>();
            if (hint == null) {
                Debug.LogWarning("fail to find ActionHint attach to prefab w/ index of" + noteIdx);
            }

            hint.Perish(hit);
        }

        // Inspector Fields  ###################################################

        [SerializeField]
        private Transform prefabs;

        [SerializeField]
        private float renderDistanceBeat = 10f;

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

            // calc vars  ------------------------------------------------------
            lastBeatLineOnBeat = -1;
            barlineBeatlineY = beatmap.origin.y + BARLINE_BEATLINE_OFFSET_Y;
            actionHintY = beatmap.origin.y + ACTION_HINT_OFFSET_Y;

            // local copy queue  -----------------------------------------------
            renderBeatNotesQ = new Queue<(int, float, Note)>();
            float beatCount;
            int noteIdx = 0;
            foreach (Note note in beatmap.notesQ) {
                beatCount = beatmap.CalcNoteBeat(note);
                renderBeatNotesQ.Enqueue((noteIdx, beatCount, note));
                noteIdx++;
            }
            if (renderBeatNotesQ.Count() == 0) {
                Debug.LogError("empty notesQ");
            }

            // create per-type pools  ------------------------------------------
            // action hints
            actionHintJumpPool = new(8, PREFAB_FOLDER + "ActionHintJump", prefabs);
            actionHintAttackPool = new(8, PREFAB_FOLDER + "ActionHintAttack", prefabs);
            actionHintSquatPool = new(8, PREFAB_FOLDER + "ActionHintSquat", prefabs);

            beatLinePool = new(16, PREFAB_FOLDER + "BeatLine", prefabs);
            barlinePool = new(4, PREFAB_FOLDER + "Barline", prefabs);
        }

        private void Update() {
            if ((GCS.I.states & GameState.PIECE_CONTROl) == 0) {
                return;
            }

            float currentRenderBeat = beatmap.currentBeat + renderDistanceBeat;

            // place beat line & barline  --------------------------------------
            while (lastBeatLineOnBeat < currentRenderBeat) {
                lastBeatLineOnBeat += 1;
                float x = beatmap.CalcXFromBeat(lastBeatLineOnBeat);

                if (lastBeatLineOnBeat % beatmap.meta.beatPerBar == 0) {
                    // barline
                    barlinePool.Spawn(x, barlineBeatlineY, -1);
                } else {
                    // beat lines
                    beatLinePool.Spawn(x, barlineBeatlineY, -1);
                }
            }

            // place action hints  ---------------------------------------------
            int noteIdx;
            float onBeat;
            Note note;
            while (renderBeatNotesQ.Count > 0) {
                (noteIdx, onBeat, note) = renderBeatNotesQ.Peek();
                if (onBeat < currentRenderBeat) {
                    float x = beatmap.CalcXFromBeat(onBeat);
                    if (note.type == "jump") {
                        actionHintJumpPool.Spawn(x, actionHintY, noteIdx);
                    } else if (note.type == "squat") {
                        actionHintSquatPool.Spawn(x, actionHintY, noteIdx);
                    } else {
                        actionHintAttackPool.Spawn(x, actionHintY, noteIdx);
                    }

                    renderBeatNotesQ.Dequeue();

                } else {
                    break;
                }
            }
        }

        private void OnDestroy() {
            actionHintJumpPool.Dispose();
            actionHintAttackPool.Dispose();
            actionHintSquatPool.Dispose();
        }

        // constants  ##########################################################
        private const string PREFAB_FOLDER = "Prefabs/BeatmapElements/";
        private const float BARLINE_BEATLINE_OFFSET_Y = 3.0f;
        private const float ACTION_HINT_OFFSET_Y = 6.0f;

        // private members  ####################################################
        // note beat, note object
        private Queue<(int, float, Note)> renderBeatNotesQ;
        private int lastBeatLineOnBeat;
        private float barlineBeatlineY;
        private float actionHintY;

        // per-element pools
        private PrefabPool<int> actionHintJumpPool;
        private PrefabPool<int> actionHintAttackPool;
        private PrefabPool<int> actionHintSquatPool;
        private PrefabPool<int> beatLinePool;
        private PrefabPool<int> barlinePool;

        // Cached Reference
        private Beatmap beatmap;
    }
}
