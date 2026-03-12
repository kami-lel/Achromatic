using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;


public class PlayerInputManager {
    // public methods  =========================================================

    public void SetInputForExplorePlay() {
        playerInput.SwitchCurrentActionMap("PlayerExplorePlay");
        playerCollider.sharedMaterial = p.defaultMaterial;
    }

    public void SetInputForMusicPlay() {
        playerInput.SwitchCurrentActionMap("PlayerMusicPlay");
        playerCollider.sharedMaterial = p.noFrictionMaterial;
    }

    // constructor  ============================================================
    public PlayerInputManager(PlayerScript parent) {
        p = parent;

        playerInput = p.GetComponent<PlayerInput>();
        playerCollider = p.GetComponent<Collider2D>();
    }

    // MonoBehavior Lifecycle  =================================================

    public void Start() {
        playerInput.defaultActionMap = "PlayerExplorePlay";
        SetInputForExplorePlay();
        playerInput.onActionTriggered += OnActionTriggered;
    }


    public void OnDisable() {
        playerInput.onActionTriggered -= OnActionTriggered;
    }

    // private members  ========================================================
    // cached references
    private readonly PlayerScript p;
    private readonly Collider2D playerCollider;

    private PlayerInput playerInput;

    // private methods  ========================================================

    private void OnActionTriggered(InputAction.CallbackContext ctxt) {
        if ((GCS.I.states & GameState.EXPLORE_CONTROL) == 0) {
            return;
        }

        switch (ctxt.action.phase) {
        case InputActionPhase.Started:  // -------------------------------------
            switch (ctxt.action.name) {
            case "Jump":
                p.mvmt.Jump();
                break;

            case "Left":
                p.mvmt.TurnLeft();
                break;

            case "Right":
                p.mvmt.TurnRight();
                break;

            case "Squat":
                p.Squat();
                break;

            case "Interact":
                Debug.Log("Player:\tInteract!!!");  // todo implement explore interaction
                break;

            }
            break;

        case InputActionPhase.Canceled:  // ------------------------------------
            switch (ctxt.action.name) {
            case "Left":
            case "Right":
                p.mvmt.Stop();
                break;
            }
            break;
        }
    }

}