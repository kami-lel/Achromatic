
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

            player.TurnRight();
            player.SetInputForMusicPlay();
            player.AnimationStartWalk();

            // calc movement during prelude  -----------------------------------
            float t = p.beatmap.meta.preludeSeconds;
            if (t <= 0f) {
                Debug.LogError("preludeSeconds must be > 0");  // prevent div by zero
                t = Mathf.Epsilon;
            }
            float s = p.mainPieceOrigin.x - p.preludeStartOrigin.x;
            float v = p.beatmap.horizontalSpeedInMainPiece;

            // calc init velocity
            float u = 2f * s / t - v;
            if (u < 0f) {
                Debug.LogWarning("must be larger distance during prelude");
                u = 0f;
            }
            playerRB.linearVelocityX = u;

            // calc acceleration — use v - u over t to be explicit
            float acceleration = (v - u) / t;
            forceDuringPrelude = new Vector2(acceleration * playerRB.mass, 0f);
        }

        public void StartMainPiece() {
            // BUG no one is calling this
            Debug.Log("PlayerManager: StartMainPiece");

            playerRB.bodyType = RigidbodyType2D.Kinematic;
            playerInput.SwitchCurrentActionMap("PlayerMusicPlay");

            isDuringMainPiece = true;
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
                Debug.Log(forceDuringPrelude);
                playerRB.AddForce(forceDuringPrelude);
            }
        }

        // constructor  ########################################################
        public PlayerManager(PieceScript piece) {
            this.p = piece;

            // find player
            GameObject playerObject = GameObject.FindWithTag(PLAYER_TAG);
            if (playerObject == null) {
                Debug.LogError("fail to find GameObject with tag 'player'");
                return;
            }

            player = playerObject.GetComponent<PlayerScript>();
            playerInput = playerObject.GetComponent<PlayerInput>();
            playerRB = player.playerRB;
        }

        // constants  ##########################################################

        private const string PLAYER_TAG = "Player";

        // private members  ####################################################
        private bool isDuringPrelude = false;
        private bool isDuringMainPiece = false;
        private Vector2 forceDuringPrelude;

        // cached references
        private readonly PieceScript p;

    }
}