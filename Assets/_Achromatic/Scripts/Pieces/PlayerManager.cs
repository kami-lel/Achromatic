
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets._Achromatic.Scripts.Pieces {

    /// <summary>
    /// take control of player GameObject during music piece
    /// </summary>
    public class PlayerManager {

        // public members  #####################################################

        // cached references
        public PlayerScript player;
        public PlayerInput playerInput;

        [NonSerialized]
        public Rigidbody2D playerRB;

        // public methods  #####################################################

        public void StartPrelude() {
            tmpPlayerLastJump = Time.time;

            Debug.Log("PlayerManager:\tStartPrelude");

            GCS.I.states = GameState.PRELUDE;

            playerRB.linearVelocityX = preludeStartVelocityX;

            player.EnsureFacingRight();
            player.im.SetInputForMusicPlay();
            player.StartRun();
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

        public void tmpJump() { // hack tmp jump
            tmpPlayerLastJump = Time.time;
        }

        // MonoBehavior Lifecycle  #############################################

        public void Update() {
            // main piece  -----------------------------------------------------
            if (GCS.I.states == GameState.MAIN_PIECE) {

                // Todo use Spline path

                float y = p.beatmap.origin.y +
                        p.tmpJumpCurve.Evaluate(Time.time - tmpPlayerLastJump);
                float x = p.beatmap.CalcCurrentXFromBeat();

                Vector2 newPosition = new(x, y);
                playerRB.MovePosition(newPosition);
            }
        }

        public void FixedUpdate() {
            // prelude  --------------------------------------------------------
            if (GCS.I.states == GameState.PRELUDE) {
                // fixme using music to control triggering
                if (p.music.Time >= p.beatmap.meta.preludeSeconds) {
                    StartMainPiece();
                    return;
                }

                playerRB.linearVelocityX += preludeAcceleration * Time.fixedDeltaTime;
            }
        }

        // constructor  ########################################################
        public PlayerManager(PieceScript piece) {
            p = piece;

            // find player
            GameObject playerObject = GameObject.FindWithTag(PLAYER_TAG);
            if (playerObject == null) {
                Debug.LogError("PlayerManager:\tfail to find GameObject with tag 'player'");
                return;
            }

            player = playerObject.GetComponent<PlayerScript>();
            playerInput = playerObject.GetComponent<PlayerInput>();
            playerRB = player.playerRB;

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
        private float tmpPlayerLastJump;


        // cached references
        private readonly PieceScript p;

    }
}