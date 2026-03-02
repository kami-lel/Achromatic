
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets._Achromatic.Scripts.Pieces {

    /// <summary>
    /// take control of player GameObject during music piece
    /// </summary>
    public class PlayerManager {

        public PlayerScript player;
        public PlayerInput playerInput;

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

        private const string PLAYER_TAG = "Player";


        public void StartControlPlayer() {

        }

        public void Update() {

        }

        public void OnDisable() {

        }

        // TODO

        // private void StartPlay() {
        //     Debug.Log("PieceScript: player enters Start Play hit box");
        //     phase = Phase.MAIN_PLAY;

        //     playerScript.SetExplorePlay(false);
        //     playerInput.SwitchCurrentActionMap("PlayerMusicPlay");
        //     playerRB.MovePosition(origin);
        //     playerScript.AnimationStartWalk();


        //     if (_timerRoutine != null)
        //         StopCoroutine(_timerRoutine);  // stop old
        //     _timerRoutine = StartCoroutine(TimerCoroutine());
        // }

        // private IEnumerator TimerCoroutine() {
        //     double targetDsp = AudioSettings.dspTime + TARGET_SECONDS;  // compute dsp target
        //     while (AudioSettings.dspTime < targetDsp) {
        //         yield return null;  // wait until dspTime reaches target
        //     }
        //     _timerRoutine = null;  // clear handle
        //     SceneManager.LoadScene("EndScene");  // perform scene change
        // }

        // private PlayerScript playerScript;
        // private Rigidbody2D playerRB;
        // private PlayerInput playerInput;

        // /// <summary>
        // /// handle update of player's control
        // /// </summary>
        // private void PlayerUpdate() {

        //     float y = -0.8345073f; // Hack
        //     // float y = origin.y + tmpJumpCurve.Evaluate(Time.time - tmpPlayerLastJump);

        //     // update user horizontal position
        //     Vector2 newPosition = new(beatmap.CalcCurrentXFromBeat(), y);
        //     playerRB.MovePosition(newPosition);
        // }

        // private void PlayerStart() {
        //     // link player references
        //     GameObject player = GameControllerScript.GetPlayer();
        //     playerRB = player.GetComponent<Rigidbody2D>();
        //     playerScript = player.GetComponent<PlayerScript>();
        //     playerInput = player.GetComponent<PlayerInput>();

        // }

        // private void PlayerOnDisable() {
        //     if (playerScript != null) {
        //         playerScript.SetExplorePlay(true);
        //     }
        //     playerInput.onActionTriggered -= OnActionTriggered;

        // }

        // private const float TARGET_SECONDS = 169f;
        // private Coroutine _timerRoutine;
    }
}