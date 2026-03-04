
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
        public Rigidbody2D playerRB;

        // public methods  #####################################################

        public void StartPrelude() {
            Debug.Log("PlayerManager: StartPrelude");

            isDuringPrelude = true;

            playerRB.linearVelocityX = preludeStartVelocityX;

            player.TurnRight();
            player.SetInputForMusicPlay();
            player.AnimationStartWalk();

        }

        public void StartMainPiece(int debugMusicStaringBar = 0) {
            Debug.Log("PlayerManager: StartMainPiece");

            isDuringMainPiece = true;
            isDuringPrelude = false;

            playerRB.bodyType = RigidbodyType2D.Kinematic;
            playerInput.SwitchCurrentActionMap("PlayerMusicPlay");
        }

        public void FinishPiece() {
            Debug.Log("PlayerManager: FinishPiece");


            isDuringMainPiece = false;
            isDuringPrelude = false;

        }

        // MonoBehavior Lifecycle  #############################################

        public void Update() {
            if (isDuringMainPiece) {  // ---------------------------------------
                // Todo use Spline path

                float y = p.beatmap.origin.y;
                float x = p.beatmap.CalcCurrentXFromBeat();

                Vector2 newPosition = new(x, y);
                playerRB.MovePosition(newPosition);
            }
        }

        public void FixedUpdate() {
            if (isDuringPrelude) {
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
                Debug.LogError("fail to find GameObject with tag 'player'");
                return;
            }

            player = playerObject.GetComponent<PlayerScript>();
            playerInput = playerObject.GetComponent<PlayerInput>();
            playerRB = player.playerRB;

            // calc movement during prelude  -----------------------------------
            float t = p.beatmap.meta.preludeSeconds;
            if (t <= 0f) {
                Debug.LogError("preludeSeconds must be > 0");  // prevent div by zero
                t = Mathf.Epsilon;
            }

            float s = p.mainPieceOrigin.x - p.preludeStartOrigin.x;
            float v = p.beatmap.horizontalSpeedInMainPiece;

            // calc init velocity
            preludeStartVelocityX = 2f * s / t - v;
            if (preludeStartVelocityX < 0f) {
                Debug.LogWarning("for prelude: must be larger distance or lower final speed");
                preludeStartVelocityX = 0f;
            }

            // calc acceleration — use v - u over t to be explicit
            preludeAcceleration = (v - preludeStartVelocityX) / t;

            Debug.Log($"prelude start speed={preludeStartVelocityX}\tacceleration={preludeAcceleration}");
        }

        // constants  ##########################################################

        private const string PLAYER_TAG = "Player";

        // private members  ####################################################
        private bool isDuringPrelude = false;
        private bool isDuringMainPiece = false;
        private float preludeStartVelocityX;
        private float preludeAcceleration;

        // cached references
        private readonly PieceScript p;

    }
}