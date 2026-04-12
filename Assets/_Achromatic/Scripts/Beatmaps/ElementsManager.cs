using System.Collections.Generic;
using System.Linq;
using Assets._Achromatic.Scripts.Scores;
using UnityEngine;


// Bug mob is missing in some attack notes

namespace Assets._Achromatic.Scripts.Beatmaps {
    [DefaultExecutionOrder(0)]
    [RequireComponent(typeof(Beatmap))]
    public class ElementsManager: MonoBehaviour {

        // Public Methods  #####################################################

        public void PerishNoteRelatedPrefab(int noteIdx, Hit hit) {
            GameObject SearchActiveQInPool(
                    PrefabPool<int> prefabPool) {
                var enumerator = prefabPool.activeQ.GetEnumerator();
                while (enumerator.MoveNext()) {
                    (int i, GameObject go) = enumerator.Current;
                    if (i == noteIdx) {
                        return go;
                    }
                }
                return null;
            }

            void PerishHint(GameObject go) {  // perish hint go
                if (go == null)
                    return;
                if (go.TryGetComponent(out ActionHint hint)) {
                    hint.Perish(hit);
                } else {
                    Debug.LogWarning(
                        "fail to find: ActionHint on prefab w/ index of: "
                        + noteIdx, this);
                }
            }

            // routine  ********************************************************
            if (noteIdx == -1) {
                return;
            }

            // resolve hint pool  ----------------------------------------------
            GameObject hintGo =
                SearchActiveQInPool(actionHintJumpPool) ??
                SearchActiveQInPool(actionHintAttackPool) ??
                SearchActiveQInPool(actionHintSquatPool);

            if (hintGo == null) {
                Debug.LogWarning(
                    "fail to find: Action Hint Prefab w/ index of: "
                    + noteIdx, this);
                return;
            }

            PerishHint(hintGo);

            // perish paired mob if present (attack notes only)  ---------------
            GameObject mobGo = SearchActiveQInPool(mobPool);
            if (mobGo != null &&
                    mobGo.TryGetComponent(out Enemy mobHint)) {
                mobHint.Perish(hit);
            }
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
                Debug.LogError("must assign: Prefabs", this);
                return;
            }

            // caching reference of piece  -------------------------------------
            beatmap = GetComponent<Beatmap>();
            if (beatmap == null) {
                Debug.LogError("fail to get: Beatmap", this);
            }

            // calc vars  ------------------------------------------------------
            lastBeatLineOnBeat = -1;
            beatmapOriginY = beatmap.origin.y;
            barlineBeatlineY = beatmapOriginY + BARLINE_BEATLINE_OFFSET_Y;
            actionHintY = beatmapOriginY + ACTION_HINT_OFFSET_Y;

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
                Debug.LogError("empty notesQ", this);
            }

            // create per-type pools  ------------------------------------------
            // action hints
            actionHintJumpPool = new(8, PREFAB_FOLDER + "ActionHintJump", prefabs);
            actionHintAttackPool = new(8, PREFAB_FOLDER + "ActionHintAttack", prefabs);
            actionHintSquatPool = new(8, PREFAB_FOLDER + "ActionHintSquat", prefabs);

            beatLinePool = new(16, PREFAB_FOLDER + "BeatLine", prefabs);
            barlinePool = new(4, PREFAB_FOLDER + "Barline", prefabs);

            blockadePool = new(8, PREFAB_FOLDER + "blockade", prefabs);
            obstaclePool = new(8, PREFAB_FOLDER + "obstacle", prefabs);
            mobPool = new(8, PREFAB_FOLDER + "Enemy", prefabs);
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
                        blockadePool.Spawn(x, beatmapOriginY, -1);
                    } else if (note.type == "squat") {
                        actionHintSquatPool.Spawn(x, actionHintY, noteIdx);
                        obstaclePool.Spawn(x, beatmapOriginY, -1);
                    } else {
                        // attack
                        actionHintAttackPool.Spawn(x, actionHintY, noteIdx);
                        mobPool.Spawn(x, beatmapOriginY, noteIdx);
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
        private const float BARLINE_BEATLINE_OFFSET_Y = 5.5f;
        private const float ACTION_HINT_OFFSET_Y = 5.5f;

        // private members  ####################################################
        // note beat, note object
        private Queue<(int, float, Note)> renderBeatNotesQ;
        private int lastBeatLineOnBeat;
        private float barlineBeatlineY;
        private float actionHintY;
        private float beatmapOriginY;

        // per-element pools
        private PrefabPool<int> actionHintJumpPool;
        private PrefabPool<int> actionHintAttackPool;
        private PrefabPool<int> actionHintSquatPool;
        private PrefabPool<int> beatLinePool;
        private PrefabPool<int> barlinePool;
        private PrefabPool<int> blockadePool;
        private PrefabPool<int> obstaclePool;
        private PrefabPool<int> mobPool;

        // Cached Reference
        private Beatmap beatmap;
    }
}

