
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
            isInControl = true;
        }

        public void Update() {
            if (!isInControl) {
                return;
            }

            // BUG BUG
            // float y = -0.8345073f;
            // float y = origin.y + tmpJumpCurve.Evaluate(Time.time - tmpPlayerLastJump);

            // update user horizontal position
            // Vector2 newPosition = new(beatmap.CalcCurrentXFromBeat(), y);
            // playerRB.MovePosition(newPosition);
        }


        // constructor  ########################################################

        public PlayerManager() {
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
        private bool isInControl = false;

    }
}