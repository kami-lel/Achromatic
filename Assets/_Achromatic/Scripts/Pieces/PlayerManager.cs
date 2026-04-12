
using UnityEngine;

using Assets._Achromatic.Scripts.Players;
using Assets._Achromatic.Scripts.Beatmaps;


namespace Assets._Achromatic.Scripts.Pieces {
    [RequireComponent(typeof(MusicManager))]
    [RequireComponent(typeof(Beatmap))]
    [RequireComponent(typeof(Piece))]
    public class PlayerManager: MonoBehaviour {

        // Public API  #########################################################

        public void SetupPlayerForPiece() {
            rb.bodyType = RigidbodyType2D.Kinematic;
            pim.SetInputForMusicPlay();
            anim.EnsureFacing(true);
            anim.StartRun();
            currentActionStartTime = Time.time;
        }

        public void FinishMain() {
            rb.bodyType = RigidbodyType2D.Dynamic;

            pim.SetInputForExplorePlay();
            anim.StopRun();
        }

        // Public Methods  #####################################################

        public void Jump() {
            anim.Jump();
            currentAction = Actions.JUMP;
            currentActionStartTime = Time.time;
        }

        public void Squat() {
            anim.Squat();

            currentAction = Actions.SQUAT;
            currentActionStartTime = Time.time;
        }

        public void Attack() {
            anim.Attack();

            currentAction = Actions.ATTACK;
            currentActionStartTime = Time.time;
        }

        // Inspector Fields  ###################################################

        [SerializeField]
        private AnimationCurve jumpHeightVsTime;

        [SerializeField]
        private AnimationCurve attackOffsetVsTime;

        [SerializeField]
        private AnimationCurve squatOffsetVsTime;

        // MonoBehavior Lifecycle  #############################################
        private void Awake() {
            // caching reference of piece  -------------------------------------
            music = GetComponent<MusicManager>();
            if (music == null) {
                Debug.LogError("fail to get: MusicManager", this);
            }
            beatmap = GetComponent<Beatmap>();
            if (beatmap == null) {
                Debug.LogError("fail to get: Beatmap", this);
            }
            piece = GetComponent<Piece>();
            if (piece == null) {
                Debug.LogError("fail to get: Piece", this);
            }

            // caching references of player  -----------------------------------
            GameObject playerGO = GameController.I.FindMainPlayer();

            rb = playerGO.GetComponent<Rigidbody2D>();
            if (rb == null) {
                Debug.LogError("fail to find: Rigidbody2D", this);
            }

            anim = playerGO.GetComponent<AnimationManager>();
            if (anim == null) {
                Debug.LogError("fail to get: AnimationManager", this);
            }

            pim = playerGO.GetComponent<Players.InputManager>();
            if (pim == null) {
                Debug.LogError("fail to get: Player InputManager", this);
            }

            // find player sprite  ---------------------------------------------
            GameObject spriteGO = GameObject.FindWithTag(PLAYER_SPRITE_TAG);
            if (spriteGO != null) {
                playerSprite = spriteGO.GetComponent<Transform>();
            }
            if (playerSprite == null) {
                Debug.LogError("fail to find: Player Sprite by Tag", this);
            }

            // test inspector fields  ------------------------------------------
            if (playerSprite == null) {
                Debug.LogError("must assign: Player Sprite", this);
            }
            if (jumpHeightVsTime == null) {
                Debug.LogError("must assign: Jump Height Vs Time", this);
            }
            if (attackOffsetVsTime == null) {
                Debug.LogError("must assign: Attack Offset Vs Time", this);
            }
            if (squatOffsetVsTime == null) {
                Debug.LogError("must assign: Squat Offset Vs Time", this);
            }
        }

        private void FixedUpdate() {
            if ((GCS.I.states & GameState.MAIN_PIECE) != 0) {
                // main piece  -------------------------------------------------

                // move player in world map
                float x = beatmap.CalcCurrentXFromBeat();
                Vector2 newPosition = new(x, beatmap.origin.y);
                rb.MovePosition(newPosition);

                // make player movement by animation curve
                float localX = 0;
                float localY = 0;
                switch (currentAction) {
                case Actions.JUMP:
                    localY = jumpHeightVsTime.Evaluate(Time.time - currentActionStartTime);
                    break;

                case Actions.SQUAT:
                    localX = -squatOffsetVsTime.Evaluate(Time.time - currentActionStartTime);
                    break;

                case Actions.ATTACK:
                    localX = attackOffsetVsTime.Evaluate(Time.time - currentActionStartTime);
                    break;

                default:
                    break;
                }

                playerSprite.localPosition = new Vector2(localX, localY);
            }
        }


        // constants  ##########################################################
        private const string PLAYER_SPRITE_TAG = "PlayerSprite";

        // private members  ####################################################
        private float currentActionStartTime;
        private Actions currentAction;

        // cached references
        private Rigidbody2D rb;
        private AnimationManager anim;
        private MusicManager music;
        private Beatmap beatmap;
        private Players.InputManager pim;
        private Transform playerSprite;
        private Piece piece;

    }
}