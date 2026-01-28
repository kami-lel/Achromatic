using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript: MonoBehaviour {

    // Inspector Fields  #######################################################

    [SerializeField]
    private float gravityScale = 1.0f;

    [SerializeField]
    private float jumpForce = 5.0f;

    [SerializeField]
    private LayerMask groundLayerMask;

    [Header("Horizontal Movement")]

    [SerializeField]
    private float maxWalkingSpped;

    [SerializeField]
    private AnimationCurve walkingSpeedUpCurve;

    [SerializeField]
    private AnimationCurve walkingSlowDownCurve;

    // properties  #############################################################
    [NonSerialized]
    public PlayerInputManager inputManager;

    // whether controlled by PieceScript
    [NonSerialized]
    public bool controlledByPiece = false;

    private Rigidbody2D playerRB;
    private PlayerInput playerInput;
    private WalkingState walkingState = WalkingState.STOP;
    private float walkingElapseTime = 0.0f;

    // MonoBehavior Lifecycle  #################################################
    public void Start() {
        playerRB = GetComponent<Rigidbody2D>();
        playerRB.bodyType = RigidbodyType2D.Dynamic;
        playerRB.gravityScale = gravityScale;
        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void OnEnable() {
        // subscribe to input system
        playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnActionTriggered;

        // HACK
        // inputManager = new(GetComponent<PlayerInput>());
    }

    public void OnDisable() {
        // unsubscribe
        playerInput.onActionTriggered -= OnActionTriggered;

        // HACK
        // inputManager?.Dispose();
        // inputManager = null;
    }

    private void Update() {
        // walking  ============================================================
        if (walkingState != WalkingState.STOP) {
            // TODO
        }

    }

    // player movement  ########################################################
    // during explore play

    [Flags]
    private enum WalkingState {
        STOP = 0,
        SPEED_UP_LEFT = 1 << 0,
        SLOW_DOWN_LEFT = 1 << 2,
        SPEED_UP_RIGHT = 1 << 3,
        SLOW_DOWN_RIGHT = 1 << 4,
        SPEED_UP = SPEED_UP_LEFT | SPEED_UP_RIGHT,
        SLOW_DOWN = SLOW_DOWN_LEFT | SLOW_DOWN_RIGHT,
    }


    /// <summary>
    /// event handler for inputs during <b>explore play</b>
    /// </summary>
    private void OnActionTriggered(InputAction.CallbackContext ctxt) {
        switch (ctxt.action.phase) {
        case InputActionPhase.Started:  // -------------------------------------
            switch (ctxt.action.name) {
            case "Jump":
                Jump();
                break;

            case "Left":
                walkingState = WalkingState.SPEED_UP_LEFT;
                walkingElapseTime = 0.0f;
                break;

            case "Right":
                walkingState = WalkingState.SPEED_UP_RIGHT;
                walkingElapseTime = 0.0f;
                break;

            case "Dash":
                Dash();
                break;

            case "Interact":
                Interact();
                break;

            }
            break;

        case InputActionPhase.Canceled:  // ------------------------------------

            switch (ctxt.action.name) {
            case "Left":
                walkingState = WalkingState.SLOW_DOWN_LEFT;
                break;

            case "Right":
                walkingState = WalkingState.SLOW_DOWN_RIGHT;

                break;
            }
            break;

        }
    }

    /// <summary>
    /// player jump (during <i>explore play</i>)
    /// </summary>
    private void Jump() {
        if (!playerRB.IsTouchingLayers(groundLayerMask))
            return;

        playerRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// player dash (during <i>explore play</i>)
    /// </summary>
    private void Dash() {
        Debug.Log("DASH");  // TODO
    }

    /// <summary>
    /// player main interact (during <i>explore play</i>)
    /// </summary>
    private void Interact() {
        Debug.Log("Interact");  // TODO
    }
}


