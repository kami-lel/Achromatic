
using UnityEngine;
using UnityEngine.InputSystem;
using Assets._Achromatic.Scripts.Players;
using Assets._Achromatic.Scripts.Scores;
using Assets._Achromatic.Scripts.Beatmaps;


namespace Assets._Achromatic.Scripts.Pieces {
    [RequireComponent(typeof(Criteria))]
    [RequireComponent(typeof(Score))]
    [RequireComponent(typeof(PlayerManager))]
    [RequireComponent(typeof(ElementsManager))]
    public class InputManager: MonoBehaviour {

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            pressed = Actions.NONE;

            // caching references to piece  ------------------------------------
            score = GetComponent<Score>();
            if (score == null) {
                Debug.LogError("fail to get: Score");
            }
            criteria = GetComponent<Criteria>();
            if (score == null) {
                Debug.LogError("fail to get: Criteria");
            }
            playerManager = GetComponent<PlayerManager>();
            if (playerManager == null) {
                Debug.LogError("fail to get: Player Manager");
            }
            elementsManager = GetComponent<ElementsManager>();
            if (elementsManager == null) {
                Debug.LogError("fail to get: Elements Manager");
            }

            // caching references to player  -----------------------------------
            GameObject go = GCS.FindPlayer();
            pi = go.GetComponent<PlayerInput>();

            if (pi == null) {
                Debug.LogError("fail to find: Player Input");
            }
        }

        private void Start() {
            if (pi != null) {
                pi.onActionTriggered += OnActionTriggered;
            } else {
                Debug.LogError("fail to subscribe");
            }
        }

        public void OnDisable() {
            if (pi != null) {
                pi.onActionTriggered -= OnActionTriggered;
            }
        }

        // event handlers  #####################################################
        private void OnActionTriggered(InputAction.CallbackContext ctxt) {
            if ((GCS.I.states & GameState.PIECE_CONTROl) == 0) {
                return;
            }

            InputAction a = ctxt.action;

            switch (a.phase) {
            case InputActionPhase.Started:
                switch (a.name) {
                case "Jump":
                    pressed |= Actions.JUMP;
                    break;
                case "Squat":
                    pressed |= Actions.SQUAT;
                    break;
                case "Attack":
                    pressed |= Actions.ATTACK;
                    break;
                case "Trigger":
                    Trigger();
                    break;
                }
                break;

            case InputActionPhase.Canceled:
                switch (a.name) {
                case "Jump":
                    pressed &= ~Actions.JUMP;
                    break;
                case "Squat":
                    pressed &= ~Actions.SQUAT;
                    break;
                case "Attack":
                    pressed &= ~Actions.ATTACK;
                    break;
                }
                break;
            }
        }

        // private members  ####################################################
        private Actions pressed;

        // cached references
        private PlayerInput pi;
        private Score score;
        private Criteria criteria;
        private PlayerManager playerManager;
        private ElementsManager elementsManager;

        // private methods  ####################################################

        private void Trigger() {
            (Hit hit, int noteIdx) = criteria.Judge(pressed);
            score.Record(hit);
            SFX.I.OnHit(pressed, hit);
            elementsManager.PerishActionHint(noteIdx, hit);

            if ((pressed & Actions.JUMP) != 0) {
                playerManager.Jump();
            } else if ((pressed & Actions.SQUAT) != 0) {
                playerManager.Squat();
            } else if ((pressed & Actions.ATTACK) != 0) {
                playerManager.Attack();
            }
        }
    }
}

