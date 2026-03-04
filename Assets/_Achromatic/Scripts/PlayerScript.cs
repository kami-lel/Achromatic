using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.IK;

// Todo 3rd actions
// Bug fix dash during animations

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerScript: MonoBehaviour {

    // public members  #########################################################

    public Rigidbody2D playerRB;
    public event Action<String> OnTriggerEnter;
    public event Action<String> OnTriggerExit;

    // public methods  #########################################################

    // movements public methods  ===============================================

    public void TurnLeft() {
        moveDir = -1;
        MovementEnsureFacing(-1);
        AnimationStartWalk();
    }

    public void TurnRight() {
        moveDir = 1;
        MovementEnsureFacing(1);
        AnimationStartWalk();
    }

    public void MovementDash() {
        if (!IsOnGround()) {
            return;
        }

        AnimationDash();
    }

    public void StopMovement() {
        moveDir = 0;
        animator.SetBool(IN_MOVEMENT_ID, false);
    }

    public void MovementJump() {
        if (!isControllingPlayer || !IsOnGround())
            return;

        playerRB.AddForce(Vector2.up * JUMP_FORCE, ForceMode2D.Impulse);

        AnimationJump();
    }

    public bool IsOnGround() {
        return playerRB.IsTouchingLayers(groundLayerMask);
    }

    // animation public methods  ===============================================

    public void AnimationDash() {
        Debug.Log("Dash");

        // Hack need animation for dash
        squashTargetY = 0.35f;  // set Target Y value
        squashDuration = 0.5f;  // set Duration value
        timer = squashDuration;  // reset Timer
        is_squashed = true;  // enable restore logic
        Vector3 s = tmpPlayerSprite.transform.localScale;  // read current scale
        s.y = squashTargetY;  // assign squashed Y
        tmpPlayerSprite.transform.localScale = s;  // apply immediate squash

        SFXManagerScript.Instance.PlayDashSFX();
    }

    public void AnimationJump() {
        SFXManagerScript.Instance.PlayJumpSFX();
        animator.SetTrigger(JUMP_ID);
    }

    public void AnimationStartWalk() {
        animator.SetBool(IN_MOVEMENT_ID, true);
    }

    // input public methods  ===================================================

    public void SetInputForExplorePlay() {
        playerInput.SwitchCurrentActionMap("PlayerExplorePlay");
        playerInput.onActionTriggered += OnActionTriggered;
        isControllingPlayer = true;
    }

    public void SetInputForMusicPlay() {
        playerInput.SwitchCurrentActionMap("PlayerMusicPlay");
        playerInput.onActionTriggered -= OnActionTriggered;
        isControllingPlayer = false;
    }

    // Inspector Fields  #######################################################

    [SerializeField]
    private LayerMask groundLayerMask = Physics2D.AllLayers;

    [SerializeField]
    private GameObject tmpPlayerSprite;  // Hack

    // MonoBehavior Lifecycle  #################################################

    void Awake() {
        playerRB = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        // Hack rm
        tmpOriginalScale = tmpPlayerSprite.transform.localScale;
    }

    private void Start() {
        playerRB.bodyType = RigidbodyType2D.Dynamic;
        playerRB.gravityScale = GRAVITY_SCALE;
        playerRB.freezeRotation = true;

        playerInput.defaultActionMap = "PlayerExplorePlay";
        SetInputForExplorePlay();
    }

    void FixedUpdate() {
        MovementFixedUpdate();
    }

    private void Update() {
        // Hack rm
        if (!is_squashed)
            return;  // skip when not squashed
        timer -= Time.deltaTime;  // decrement Timer each frame
        if (timer <= 0f) {
            tmpPlayerSprite.transform.localScale = tmpOriginalScale;  // restore Original Scale
            is_squashed = false;  // clear flag
            timer = 0f;  // clear timer
        }
    }

    private void OnDisable() {
        playerInput.onActionTriggered -= OnActionTriggered;
    }

    // Unity Messages  #########################################################

    private void OnTriggerEnter2D(Collider2D other) {
        if (other == null || !other.isTrigger) {
            return;
        }

        Debug.Log("player enters trigger: " + other.tag);
        OnTriggerEnter?.Invoke(other.tag);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other == null || !other.isTrigger) {
            return;
        }

        Debug.Log("player exit trigger: " + other.tag);
        OnTriggerExit?.Invoke(other.tag);
    }

    // input  ##################################################################

    private PlayerInput playerInput;
    private bool isControllingPlayer = true;

    private void OnActionTriggered(InputAction.CallbackContext ctxt) {
        if (!isControllingPlayer) {
            return;
        }

        switch (ctxt.action.phase) {
        case InputActionPhase.Started:  // -------------------------------------
            switch (ctxt.action.name) {
            case "Jump":
                MovementJump();
                break;

            case "Left":
                TurnLeft();
                break;

            case "Right":
                TurnRight();
                break;

            case "Dash":
                MovementDash();
                break;

            case "Interact":
                Debug.Log("Interact!!!");  // todo implement explore interaction
                break;

            }
            break;

        case InputActionPhase.Canceled:  // ------------------------------------
            switch (ctxt.action.name) {
            case "Left":
            case "Right":
                StopMovement();
                break;
            }
            break;
        }
    }

    // movements  ##############################################################

    readonly private float GRAVITY_SCALE = 1.0f;
    readonly private float JUMP_FORCE = 8.0f;
    readonly private float MAX_WALKING_SPEED = 15.0f;
    private bool isFacingRight = true;
    private int moveDir = 0;

    private void MovementFixedUpdate() {
        if (!isControllingPlayer) {
            return;
        }

        // apply horizontal force toward target velocity
        float targetVelX = moveDir * MAX_WALKING_SPEED;
        float velDiff = targetVelX - playerRB.linearVelocityX;
        float requiredAccel = velDiff / Time.fixedDeltaTime;
        float maxForce = Mathf.Abs(requiredAccel * playerRB.mass);
        // clamp force to avoid extreme impulses
        float forceX = Mathf.Clamp(requiredAccel * playerRB.mass,
            -maxForce, maxForce);
        playerRB.AddForce(new Vector2(forceX, 0f));

        // light damping when idle to reduce sliding
        if (moveDir == 0 && Mathf.Abs(playerRB.linearVelocityX) < 0.01f) {
            playerRB.linearVelocity = new Vector2(0f, playerRB.linearVelocityY);
        }

    }

    // helpers  ================================================================

    private void MovementEnsureFacing(int dir) {
        if (dir == 0)
            return;
        bool shouldFaceRight = dir > 0;
        if (shouldFaceRight != isFacingRight)
            MovementFlip();
    }

    private void MovementFlip() {
        isFacingRight = !isFacingRight;
        Vector3 s = transform.localScale;
        s.x = -s.x;
        transform.localScale = s;
    }

    // animations  #############################################################

    // constants  ==============================================================
    private readonly int IN_MOVEMENT_ID = Animator.StringToHash("InMovement");
    private readonly int JUMP_ID = Animator.StringToHash("Jump");

    private Animator animator;
    // Hack tmp vars
    private Vector3 tmpOriginalScale;
    private float timer = 0.0f;
    private float squashDuration = 0.5f;  // default Duration seconds
    private float squashTargetY = 0.1f;  // default Target Y scale
    private bool is_squashed = false;  // flag Squash Active

}