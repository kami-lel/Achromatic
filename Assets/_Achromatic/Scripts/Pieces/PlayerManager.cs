
using System;
using Assets._Achromatic.Scripts.Scores;
using UnityEngine;
using UnityEngine.InputSystem;

using Assets._Achromatic.Scripts.Players;

namespace Assets._Achromatic.Scripts.Pieces {

    public class PlayerManager: MonoBehaviour {

        // Public API  #########################################################

        public void StartPrelude() {
            playerLastActionTime = Time.time;

            Debug.Log("PlayerManager:\tStartPrelude");

            rb.linearVelocityX = preludeStartVelocityX;

            anim.EnsureFacing(true);
            // HACK
            // player.im.SetInputForMusicPlay();
            // player.StartRun();
        }

        public void StartMainPiece(int debugMusicStaringBar = 0) {
            // TODO consider game state when scene transition
            Debug.Log("PlayerManager:\tStartMainPiece");

            GCS.I.states = GameState.MAIN_PIECE;

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
            // HACK
            // playerLastActionTime = Time.time;
            // actionType = 1;
        }

        public void Squat(Hit hit) {
            anim.Squat();
            SFX.I.Squat(hit);
            // HACK
            // playerLastActionTime = Time.time;
            // actionType = 2;
        }

        public void Attack(Hit hit) {
            anim.Attack();
            SFX.I.Attack(hit);
            // HACK
            // playerLastActionTime = Time.time;
            // actionType = 3;
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
            // caching references  ---------------------------------------------
            GameObject go = GCS.FindPlayer();

            rb = go.GetComponent<Rigidbody2D>();
            if (rb == null) {
                Debug.LogError("fail to find: Rigidbody2D");
            }

            anim = go.GetComponent<AnimationManager>();
            if (anim == null) {
                Debug.LogError("fail to get: AnimationManager");
            }

            music = go.GetComponent<MusicManager>();
            if (music == null) {
                Debug.LogError("fail to get: MusicManager");
            }

            pi = go.GetComponent<PlayerInput>();
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

                // make player movement by animation curve
                float localX = 0;
                float localY = 0;
                switch (actionType) {
                case 1:
                    localY = jumpHeightVsTime.Evaluate(Time.time - playerLastActionTime);
                    break;

                case 2:
                    localX = -squatOffsetVsTime.Evaluate(Time.time - playerLastActionTime);
                    break;

                case 3:
                    localX = attackOffsetVsTime.Evaluate(Time.time - playerLastActionTime);
                    break;

                default:
                    break;
                }

                playerSprite.localPosition = new Vector2(localX, localY);
                */
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
        private float playerLastActionTime;
        private int actionType = 0;  // HACK better way to do this

        // cached references
        private Rigidbody2D rb;
        private AnimationManager anim;
        private MusicManager music;
        private PlayerInput pi;
        private Transform playerSprite;
    }
}