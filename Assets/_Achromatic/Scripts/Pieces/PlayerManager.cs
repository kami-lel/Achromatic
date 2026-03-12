
using System;
using Assets._Achromatic.Scripts.Scores;
using UnityEngine;
using UnityEngine.InputSystem;

using Assets._Achromatic.Scripts.Players;

namespace Assets._Achromatic.Scripts.Pieces {
    [RequireComponent(typeof(MusicManager))]
    public class PlayerManager: MonoBehaviour {

        // Public API  #########################################################

        public void StartPrelude() {
            currentActionStartTime = Time.time;

            Debug.Log("PlayerManager:\tStartPrelude");

            rb.linearVelocityX = preludeStartVelocityX;

            anim.EnsureFacing(true);
            anim.StartRun();
            // HACK
            // player.im.SetInputForMusicPlay();
        }

        public void StartMainPiece(int debugMusicStaringBar = 0) {
            // TODO consider game state when scene transition
            Debug.Log("PlayerManager:\tStartMainPiece");


            rb.bodyType = RigidbodyType2D.Kinematic;
            pi.SwitchCurrentActionMap("PlayerMusicPlay");
        }

        public void FinishPiece() {
            Debug.Log("PlayerManager:\tFinishPiece");

            GCS.I.states = GameState.PIECE_FINISHED;
        }

        // Public Methods  #####################################################
        public void Jump(Hit hit) {
            anim.Jump();
            SFX.I.Jump(hit);
            currentAction = Actions.JUMP;
            currentActionStartTime = Time.time;
        }

        public void Squat(Hit hit) {
            anim.Squat();
            SFX.I.Squat(hit);

            currentAction = Actions.SQUAT;
            currentActionStartTime = Time.time;
        }

        public void Attack(Hit hit) {
            anim.Attack();
            SFX.I.Attack(hit);

            currentAction = Actions.ATTACK;
            currentActionStartTime = Time.time;
        }

        // Inspector Fields  ###################################################

        // Fixme animation curve fine tuning
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
                Debug.LogError("fail to get: MusicManager");
            }

            // caching references of player  -----------------------------------
            GameObject playerGO = GCS.FindPlayer();

            rb = playerGO.GetComponent<Rigidbody2D>();
            if (rb == null) {
                Debug.LogError("fail to find: Rigidbody2D");
            }

            anim = playerGO.GetComponent<AnimationManager>();
            if (anim == null) {
                Debug.LogError("fail to get: AnimationManager");
            }

            pi = playerGO.GetComponent<PlayerInput>();
            if (pi == null) {
                Debug.LogError("fail to get: PlayerInput");
            }

            // find player sprite  ---------------------------------------------
            GameObject spriteGO = GameObject.FindWithTag(PLAYER_SPRITE_TAG);
            if (spriteGO != null) {
                playerSprite = spriteGO.GetComponent<Transform>();
            }
            if (playerSprite == null) {
                Debug.LogError("fail to find: Player Sprite by Tag");
            }

            // test inspector fields  ------------------------------------------
            if (playerSprite == null) {
                Debug.LogError("must assign: Player Sprite");
            }
            if (jumpHeightVsTime == null) {
                Debug.LogError("must assign: Jump Height Vs Time");
            }
            if (attackOffsetVsTime == null) {
                Debug.LogError("must assign: Attack Offset Vs Time");
            }
            if (squatOffsetVsTime == null) {
                Debug.LogError("must assign: Squat Offset Vs Time");
            }

            // calc movement during prelude  -----------------------------------
            /* HACK
            float t = p.beatmap.meta.preludeSeconds;
            if (t <= 0f) {
                Debug.LogError("PlayerManager:\tpreludeSeconds must be > 0");  // prevent div by zero
                t = Mathf.Epsilon;
            }

            float s = p.beatmap.origin.x - p.preludeStartOrigin.x;
            float v = p.beatmap.horizontalSpeedInMainPiece;

            // calc init velocity
            preludeStartVelocityX = 2f * s / t - v;
            if (preludeStartVelocityX < 0f) {
                Debug.LogWarning("PlayerManager:\tfor prelude: must be larger distance or lower final speed");
                preludeStartVelocityX = 0f;
            }

            // calc acceleration — use v - u over t to be explicit
            preludeAcceleration = (v - preludeStartVelocityX) / t - 0.1f;

            Debug.Log($"PlayerManager:\tprelude start speed={preludeStartVelocityX}\tacceleration={preludeAcceleration}");
            */
        }

        private void Update() {
            // main piece  -----------------------------------------------------
            if (GCS.I.states == GameState.MAIN_PIECE) {

                // Todo use Spline path

                /* HACK
                // move player in world map
                float x = p.beatmap.CalcCurrentXFromBeat();
                Vector2 newPosition = new(x, p.beatmap.origin.y);
                rb.MovePosition(newPosition);
                */

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

        private void FixedUpdate() {
            // prelude  --------------------------------------------------------
            if (GCS.I.states == GameState.PRELUDE) {
                // Fixme using music to control triggering
                // HACK
                // if (music.Time >= p.beatmap.meta.preludeSeconds) {
                //     StartMainPiece();
                //     return;
                // }

                rb.linearVelocityX += preludeAcceleration * Time.fixedDeltaTime;
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
        private PlayerInput pi;
        private Transform playerSprite;
    }
}