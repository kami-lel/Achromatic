using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript: MonoBehaviour {

    // constants  ##############################################################

    private readonly float GRAVITY_SCALE = 5.0f;


    // Inspector Fields  #######################################################
    [SerializeField]
    private float WalkingSpeed = 0.1f;

    [SerializeField]
    private GameObject circle;

    // properties  #############################################################
    [NonSerialized]
    public PlayerInputManager inputManager;

    // whether controlled by PieceScript
    public bool controlledByPiece = false;

    private Rigidbody2D playerRB;  // set as kinematic
    private PlayerInput playerInput;

    // MonoBehavior Lifecycle  #################################################
    public void Start() {
        playerRB = GetComponent<Rigidbody2D>();
        playerRB.bodyType = RigidbodyType2D.Kinematic;
        playerRB.gravityScale = GRAVITY_SCALE;
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

    // player movement  ########################################################
    // during explore play

    /// <summary>
    /// event handler for inputs during <b>explore play</b>
    /// </summary>
    private void OnActionTriggered(InputAction.CallbackContext ctxt) {
        InputAction act = ctxt.action;

        if (act.phase != InputActionPhase.Started)
            return;

        switch (act.name) {
        case "Jump":
            Jump();
            break;

        case "Left":
            Left();
            break;

        case "Right":
            Right();
            break;

        case "Dash":
            Dash();
            break;

        case "Interact":
            Interact();
            break;

        default:  // no op for other actions
            break;
        }
    }

    /// <summary>
    /// player jump (during <i>explore play</i>)
    /// </summary>
    private void Jump() {
        if (!IsOnFloor())
            return;  // BUG

        playerRB.AddForce(new Vector2(0.0f, 10.0f), ForceMode2D.Impulse);
        Debug.Log("JUMP");  // TODO
    }

    /// <summary>
    /// player turn left (during <i>explore play</i>)
    /// </summary>
    private void Left() {
        Debug.Log("LEFT");  // TODO
    }

    /// <summary>
    /// player turn right (during <i>explore play</i>)
    /// </summary>
    private void Right() {
        Debug.Log("RIGHT");  // TODO
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

    // helper functions  =======================================================
    private bool IsOnFloor() {
        return playerRB.IsTouchingLayers(Physics2D.AllLayers);
    }




    // TODO normal player exploration movement
}
