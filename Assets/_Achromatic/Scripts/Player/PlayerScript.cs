using System;
using UnityEngine;
using UnityEngine.InputSystem;

// todo implements walking (vs running)
// TODO refactorization using player movement

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerScript: MonoBehaviour {

    // public members  #########################################################

    [NonSerialized]
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
        if (!IsOnGround())
            return;

        playerRB.AddForce(Vector2.up * JUMP_FORCE, ForceMode2D.Impulse);

        AnimationJump();
    }

    public bool IsOnGround() {
        return playerRB.IsTouchingLayers(groundLayerMask);
    }

    // animation public methods  ===============================================

    public void AnimationDash() {
        // HACK need animation for dash
        squashDuration = 0.5f;  // set Duration value
        timer = squashDuration;  // reset Timer

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
        playerCollider.sharedMaterial = defaultMaterial;
    }

    public void SetInputForMusicPlay() {
        playerInput.SwitchCurrentActionMap("PlayerMusicPlay");
        playerCollider.sharedMaterial = noFrictionMaterial;
    }

    // Inspector Fields  #######################################################

    [SerializeField]
    private LayerMask groundLayerMask = Physics2D.AllLayers;

    [SerializeField]
    private PhysicsMaterial2D defaultMaterial;

    [SerializeField]
    private PhysicsMaterial2D noFrictionMaterial;

    // MonoBehavior Lifecycle  #################################################

    void Awake() {
        playerRB = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Start() {
        playerRB.bodyType = RigidbodyType2D.Dynamic;
        playerRB.gravityScale = GRAVITY_SCALE;
        playerRB.freezeRotation = true;
        playerRB.linearDamping = 0.0f;

        playerInput.defaultActionMap = "PlayerExplorePlay";
        SetInputForExplorePlay();
        playerInput.onActionTriggered += OnActionTriggered;

        GCS.I.states = GameState.EXPLORE;
    }

    void FixedUpdate() {
        if ((GCS.I.states & GameState.EXPLORE_CONTROL) == 0) {
            return;
        }

        MovementFixedUpdate();
    }

    private void OnDisable() {
        playerInput.onActionTriggered -= OnActionTriggered;
    }

    // Unity Messages  #########################################################

    private void OnTriggerEnter2D(Collider2D other) {
        if ((GCS.I.states & GameState.EXPLORE_CONTROL) == 0 ||
                other == null || !other.isTrigger) {
            return;
        }

        Debug.Log("Player:\tenters trigger: " + other.tag);
        OnTriggerEnter?.Invoke(other.tag);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other == null || !other.isTrigger) {
            return;
        }

        Debug.Log("Player:\texit trigger: " + other.tag);
        OnTriggerExit?.Invoke(other.tag);
    }

    // input  ##################################################################

    private PlayerInput playerInput;

    private void OnActionTriggered(InputAction.CallbackContext ctxt) {
        if ((GCS.I.states & GameState.EXPLORE_CONTROL) == 0) {
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

            case "Squat":
                MovementDash();
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
                StopMovement();
                break;
            }
            break;
        }
    }

    // movements  ##############################################################

    readonly private float GRAVITY_SCALE = 1.0f;
    readonly private float JUMP_FORCE = 8.0f;
    readonly private float MAX_WALKING_SPEED = 10.0f;

    private bool isFacingRight = true;
    private int moveDir = 0;

    private Collider2D playerCollider;

    private void MovementFixedUpdate() {

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

    // Hack tmp vars
    private Vector3 tmpOriginalScale;
    private float timer = 0.0f;
    private float squashDuration = 0.5f;  // default Duration seconds

    private Animator animator;

}