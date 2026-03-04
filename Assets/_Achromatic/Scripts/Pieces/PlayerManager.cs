
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
        }

        // MonoBehavior Lifecycle  #############################################

        public void Update() {
            if (isDuringPrelude) {  // -----------------------------------------
                float timeFactor =
                        piece.music.Time / piece.beatmap.meta.preludeSeconds;

                if (timeFactor >= 1.0f) {
                    StartMainPiece();
                    return;
                }

                float speed =
                        piece.preludeTimeVsSpeed.Evaluate(timeFactor) *
                        piece.beatmap.horizontalSpeedInMainPiece;

                Debug.Log(speed);  // HACK HACK

                playerRB.linearVelocityX = speed;

            } else if (isDuringMainPiece) {  // --------------------------------
                // Todo use Spline path

                float y = piece.beatmap.origin.y;
                float x = piece.beatmap.CalcCurrentXFromBeat();

                Vector2 newPosition = new(x, y);
                playerRB.MovePosition(newPosition);
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

        // cached references
        private readonly PieceScript piece;

        // private methods  ####################################################

        private void StartMainPiece() {
            Debug.Log("PlayerManager: StartMainPiece");

            playerRB.bodyType = RigidbodyType2D.Kinematic;
            playerInput.SwitchCurrentActionMap("PlayerMusicPlay");

            isDuringMainPiece = true;
            isDuringPrelude = false;
        }
    }
}