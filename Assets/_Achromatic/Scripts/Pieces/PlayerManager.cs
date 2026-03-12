
using System;
using Assets._Achromatic.Scripts.Scores;
using UnityEngine;
using UnityEngine.InputSystem;

using Assets._Achromatic.Scripts.Players;

namespace Assets._Achromatic.Scripts.Pieces {
    /// <summary>
    /// take control of player GameObject during music piece
    /// </summary>
    public class PlayerManager {

        // public members  #####################################################

        // cached references
        public Player player;
        public PlayerInput playerInput;

        [NonSerialized]
        public Rigidbody2D playerRB;

        // public methods  #####################################################

        public void StartPrelude() {
            playerLastActionTime = Time.time;

            Debug.Log("PlayerManager:\tStartPrelude");

            GCS.I.states = GameState.PRELUDE;

            playerRB.linearVelocityX = preludeStartVelocityX;

            // HACK
            // player.EnsureFacingRight();
            // player.im.SetInputForMusicPlay();
            // player.StartRun();
        }

        public void StartMainPiece(int debugMusicStaringBar = 0) {
            Debug.Log("PlayerManager:\tStartMainPiece");

            GCS.I.states = GameState.MAIN_PIECE;

            playerRB.bodyType = RigidbodyType2D.Kinematic;
            playerInput.SwitchCurrentActionMap("PlayerMusicPlay");
        }

        public void FinishPiece() {
            Debug.Log("PlayerManager:\tFinishPiece");

            GCS.I.states = GameState.PIECE_FINISHED;
        }

        public void Jump(Hit hit) {
            // HACK
            // p.playerManager.player.Jump();
            SFX.I.Jump(hit);
            playerLastActionTime = Time.time;
            actionType = 1;
        }

        public void Squat(Hit hit) {
            // HACK
            // p.playerManager.player.Squat();
            SFX.I.Squat(hit);
            playerLastActionTime = Time.time;
            actionType = 2;
        }

        public void Attack(Hit hit) {
            // HACK
            // p.playerManager.player.Attack();
            SFX.I.Attack(hit);
            playerLastActionTime = Time.time;
            actionType = 3;
        }

        // MonoBehavior Lifecycle  #############################################

        public void Update() {
            // main piece  -----------------------------------------------------
            if (GCS.I.states == GameState.MAIN_PIECE) {

                // Todo use Spline path

                // move player in world map
                float x = p.beatmap.CalcCurrentXFromBeat();
                Vector2 newPosition = new(x, p.beatmap.origin.y);
                playerRB.MovePosition(newPosition);

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
            }
        }

        public void FixedUpdate() {
            // prelude  --------------------------------------------------------
            if (GCS.I.states == GameState.PRELUDE) {
                // Fixme using music to control triggering
                if (p.music.Time >= p.beatmap.meta.preludeSeconds) {
                    StartMainPiece();
                    return;
                }

                playerRB.linearVelocityX += preludeAcceleration * Time.fixedDeltaTime;
            }
        }

        // constructor  ########################################################
        public PlayerManager(PieceScript piece, AnimationCurve jumpHeightVsTime, AnimationCurve attackOffsetVsTime, AnimationCurve squatOffsetVsTime) {
            p = piece;
            this.jumpHeightVsTime = jumpHeightVsTime;
            this.attackOffsetVsTime = attackOffsetVsTime;
            this.squatOffsetVsTime = squatOffsetVsTime;

            // find player
            GameObject playerObject = GameObject.FindWithTag(PLAYER_TAG);
            if (playerObject == null) {
                Debug.LogError("PlayerManager:\tfail to find GameObject with tag 'player'");
                return;
            }

            player = playerObject.GetComponent<PlayerScript>();
            playerInput = playerObject.GetComponent<PlayerInput>();
            playerRB = player.playerRB;
            playerSprite = p.playerSprite;

            // calc movement during prelude  -----------------------------------
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
        }

        // constants  ##########################################################

        private const string PLAYER_TAG = "Player";

        // private members  ####################################################
        private readonly float preludeStartVelocityX;

        private readonly float preludeAcceleration;

        // Fixme animation curve fine tuning
        private readonly AnimationCurve jumpHeightVsTime;
        private readonly AnimationCurve attackOffsetVsTime;
        private readonly AnimationCurve squatOffsetVsTime;
        private float playerLastActionTime;
        private int actionType = 0;  // HACK better way to do this


        // cached references
        private readonly PieceScript p;
        private readonly Transform playerSprite;

    }
}