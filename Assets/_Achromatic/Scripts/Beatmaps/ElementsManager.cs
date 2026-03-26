using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//  Todo make prefabs disappearing as feed back
namespace Assets._Achromatic.Scripts.Beatmaps {
    [DefaultExecutionOrder(0)]
    [RequireComponent(typeof(Beatmap))]
    public class ElementsManager: MonoBehaviour {

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
            lastBeatLineOnBeat = 0;
            barlineBeatlineY = beatmap.origin.y + BARLINE_BEATLINE_OFFSET_Y;
            actionHintY = beatmap.origin.y + ACTION_HINT_OFFSET_Y;

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
                lastBeatLineOnBeat += 1;
                float x = beatmap.CalcXFromBeat(lastBeatLineOnBeat);

                if (lastBeatLineOnBeat % beatmap.meta.beatPerBar == 0) {
                    // barline
                    barlinePool.Spawn(x, barlineBeatlineY);
                } else {
                    // beat lines
                    beatLinePool.Spawn(x, barlineBeatlineY);
                }
            }


            // place action hints  ---------------------------------------------
            float onBeat;
            Note note;
            while (renderBeatNotesQ.Count > 0) {
                (onBeat, note) = renderBeatNotesQ.Peek();
                if (onBeat < currentRenderBeat) {
                    float x = beatmap.CalcXFromBeat(onBeat);
                    if (note.type == "jump") {
                        actionHintJumpPool.Spawn(x, actionHintY);
                    } else if (note.type == "squat") {
                        actionHintSquatPool.Spawn(x, actionHintY);
                    } else {
                        actionHintAttackPool.Spawn(x, actionHintY);
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
        private Queue<(float, Note)> renderBeatNotesQ;
        private int lastBeatLineOnBeat;
        private float barlineBeatlineY;
        private float actionHintY;

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
