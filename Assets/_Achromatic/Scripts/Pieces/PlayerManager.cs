
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
            player.AnimationStartWalk();
            player.SetInputForMusicPlay();

            // calc acceleration during prelude
            // during prelude, accelerate the player such that
            forceDuringPrelude = new(100.0f, 50.0f); // TODO calc acceleration!
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

                float y = piece.beatmap.origin.y;
                float x = piece.beatmap.CalcCurrentXFromBeat();

                Vector2 newPosition = new(x, y);
                playerRB.MovePosition(newPosition);
            }
        }

        public void FixedUpdate() {
            if (isDuringPrelude) {
                playerRB.AddForce(forceDuringPrelude);
            }
        }

        // constructor  ########################################################
        public PlayerManager(PieceScript piece) {
            this.piece = piece;

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
        private readonly PieceScript piece;

    }
}