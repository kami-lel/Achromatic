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
    private LayerMask groundLayerMask = Physics2D.AllLayers;

    [Header("Horizontal Movement")]  // ----------------------------------------

    [SerializeField]
    private float maxWalkingSpped = 5.0f;

    [SerializeField]
    private AnimationCurve walkingSpeedUpCurve =
            AnimationCurve.EaseInOut(0.0f, 0.0f, 0.75f, 1.0f);

    [SerializeField]
    private AnimationCurve walkingSlowDownCurve =
            AnimationCurve.EaseInOut(0.0f, 1.0f, 0.75f, 0.0f);

    // public members  #########################################################

    public void SetPlayTypeAsExplore(bool isExplorePlay) {
        if (isExplorePlay) {
            playerRB.bodyType = RigidbodyType2D.Dynamic;
            playerRB.gravityScale = gravityScale;
            playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            hasSelfControl = true;
            playerInput.defaultActionMap = "PlayerExplorePlay";

        } else {
            hasSelfControl = false;
            playerInput.defaultActionMap = "PlayerMusicPlay";
        }
    }

    // private members  ########################################################
    private bool hasSelfControl;
    private Rigidbody2D playerRB;
    private PlayerInput playerInput;
    private WalkingState walkingState = WalkingState.STOP;
    private float walkingMovementElapsedTime = 0.0f;

    // MonoBehavior Lifecycle  #################################################
    public void Start() {
        playerRB = GetComponent<Rigidbody2D>();

        SetPlayTypeAsExplore(true);
    }

    public void OnEnable() {
        // subscribe to input system
        playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnActionTriggered;
        hasSelfControl = true;
    }

    public void OnDisable() {
        // unsubscribe
        playerInput.onActionTriggered -= OnActionTriggered;
    }

    private void Update() {
        if (!hasSelfControl)
            return;

        // walking  ============================================================
        // todo change to force-based
        if ((walkingState & WalkingState.SPEED_UP) != 0) {
            // speed up & sustaining walking
            playerRB.linearVelocityX =
                    walkingSpeedUpCurve.Evaluate(walkingMovementElapsedTime)
                    * maxWalkingSpped
                    * (walkingState == WalkingState.SPEED_UP_RIGHT ?
                            1.0f : -1.0f);

            walkingMovementElapsedTime += Time.deltaTime;

        } else if ((walkingState & WalkingState.SLOW_DOWN) != 0) {
            // slow down
            float curveValue = walkingSlowDownCurve.Evaluate(
                    walkingMovementElapsedTime);

            if (curveValue <= 0.0f) {
                // reach end of slowing down curve
                walkingState = WalkingState.STOP;
                playerRB.linearVelocityX = 0.0f;

            } else {
                playerRB.linearVelocityX =
                        curveValue
                        * maxWalkingSpped
                        * (walkingState == WalkingState.SLOW_DOWN_RIGHT ?
                                1.0f : -1.0f);

                walkingMovementElapsedTime += Time.deltaTime;
            }
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
                walkingMovementElapsedTime = 0.0f;
                break;

            case "Right":
                walkingState = WalkingState.SPEED_UP_RIGHT;
                walkingMovementElapsedTime = 0.0f;
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
                walkingMovementElapsedTime = 0.0f;
                break;

            case "Right":
                walkingState = WalkingState.SLOW_DOWN_RIGHT;
                walkingMovementElapsedTime = 0.0f;

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
        Debug.Log("DASH");  // todo implement dash in explore play
    }

    /// <summary>
    /// player main interact (during <i>explore play</i>)
    /// </summary>
    private void Interact() {
        Debug.Log("Interact");  // todo implement interact in explore play
    }
}


