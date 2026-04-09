
using Assets._Achromatic.Scripts.Scores;
using UnityEngine;

using Assets._Achromatic.Scripts.Players;
using Assets._Achromatic.Scripts.Beatmaps;


namespace Assets._Achromatic.Scripts.Pieces {
    [RequireComponent(typeof(MusicManager))]
    [RequireComponent(typeof(Beatmap))]
    [RequireComponent(typeof(Piece))]
    public class PlayerManager: MonoBehaviour {

        // Public API  #########################################################

        public void StartPrelude() {
            Debug.Log("PlayerManager:\tStartPrelude");

            rb.linearVelocityX = preludeStartVelocityX;

            SetupPlayerForPiece();
        }

        public void StartMainPiece(int debugMusicStaringBar = 0) {
            Debug.Log("PlayerManager:\tStartMainPiece");

            rb.bodyType = RigidbodyType2D.Kinematic;

            SetupPlayerForPiece();
        }

        public void FinishPiece() {
            Debug.Log("PlayerManager:\tFinishPiece");

            GCS.I.states = GameState.PIECE_FINISHED;
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
            GameObject playerGO = GCS.FindPlayer();

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

            // calc movement during prelude  -----------------------------------
            float t = beatmap.meta.preludeSeconds;
            if (t <= 0f) {
                // prevent div by zero
                Debug.LogError("PlayerManager:\tpreludeSeconds must be > 0", this);
                t = Mathf.Epsilon;
            }

            float s = beatmap.origin.x - piece.preludeStartOrigin.x;
            float v = beatmap.speedXInMainPiece;

            // calc init velocity
            preludeStartVelocityX = 2f * s / t - v;
            if (preludeStartVelocityX < 0f) {
                Debug.LogWarning("PlayerManager:\tfor prelude: must be larger distance or lower final speed", this);
                preludeStartVelocityX = 0f;
            }

            // calc acceleration — use v - u over t to be explicit
            preludeAcceleration = (v - preludeStartVelocityX) / t - 0.1f;

            // Debug.Log($"PlayerManager:\tprelude start speed={preludeStartVelocityX}\tacceleration={preludeAcceleration}");
        }

        private void FixedUpdate() {
            if (GCS.I.states == GameState.PRELUDE) {
                // prelude  ----------------------------------------------------
                // Fixme using music to control triggering
                if (music.Time >= beatmap.meta.preludeSeconds) {
                    StartMainPiece();
                    GCS.I.states = GameState.MAIN_PIECE;
                    return;
                }

                rb.linearVelocityX += preludeAcceleration * Time.fixedDeltaTime;
            } else if ((GCS.I.states & GameState.PIECE_CONTROl) != 0) {
                // main piece  -------------------------------------------------

                // FIXME no use Spline path

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
        private float preludeStartVelocityX;
        private float preludeAcceleration;
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



        // private methods  ####################################################

        private void SetupPlayerForPiece() {
            pim.SetInputForMusicPlay();
            anim.EnsureFacing(true);
            anim.StartRun();
            currentActionStartTime = Time.time;
        }
    }
}