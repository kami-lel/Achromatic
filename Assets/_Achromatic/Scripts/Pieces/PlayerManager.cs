
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets._Achromatic.Scripts.Pieces {

    /// <summary>
    /// take control of player GameObject during music piece
    /// </summary>
    public class PlayerManager {

        // public members  #####################################################

        public PlayerScript player;
        public PlayerInput playerInput;

        // public methods  #####################################################

        public void TakeOverPlayerControl() {
            Debug.Log("take over player control");

            player.UnsetExplorePlay();
            player.AnimationStartWalk();
        }

        public void Update() {
            if (!piece.isControllingPlayer) {
                return;
            }
            // Todo use Spline path

            float y = piece.beatmap.origin.y;
            float x = piece.beatmap.CalcCurrentXFromBeat();

            Vector2 newPosition = new(x, y);
            player.playerRB.MovePosition(newPosition);
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
        }

        // constants  ##########################################################
        private const string PLAYER_TAG = "Player";


        // private members  ####################################################
        // cached references
        PieceScript piece;
    }
}