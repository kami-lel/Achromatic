using System;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerScript: MonoBehaviour {
    // Inspector Fields  #######################################################

    [SerializeField]
    private LayerMask groundLayerMask = Physics2D.AllLayers;


    [SerializeField]
    private GameObject tmpPlayerSprite;  // hack

    // MonoBehavior Lifecycle  #################################################

    void Awake() {
        playerRB = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        // hack rm
        tmpOriginalScale = tmpPlayerSprite.transform.localScale;
    }

    private void OnEnable() {
        SetExplorePlay(true);
        playerInput.defaultActionMap = "PlayerExplorePlay";
        playerInput.onActionTriggered += OnActionTriggered;
    }

    void FixedUpdate() {
        MovementFixedUpdate();
    }

    private void Update() {
        // hack rm
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

    // public methods  #########################################################

    public void SetExplorePlay(bool in_explore_play) {
        this.in_explore_play = in_explore_play;

        if (in_explore_play) {
            playerRB.bodyType = RigidbodyType2D.Dynamic;
            playerRB.gravityScale = GRAVITY_SCALE;
            playerRB.freezeRotation = true;

            playerInput.SwitchCurrentActionMap("PlayerExplorePlay");

        } else {

            playerRB.bodyType = RigidbodyType2D.Kinematic;

            playerInput.SwitchCurrentActionMap("PlayerMusicPlay");
        }

    }

    // private members  ########################################################

    private bool in_explore_play;

    // inputs  #################################################################

    private PlayerInput playerInput;

    private void OnActionTriggered(InputAction.CallbackContext ctxt) {
        if (!in_explore_play) {
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
                Debug.Log("Interact!!!");  // todo
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

    // public methods  =========================================================

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


    // private members  ========================================================

    private Rigidbody2D playerRB;
    bool isFacingRight = true;
    int moveDir = 0;

    // constants  --------------------------------------------------------------
    readonly private float GRAVITY_SCALE = 1.0f;
    readonly private float JUMP_FORCE = 8.0f;
    readonly private float MAX_WALKING_SPEED = 15.0f;

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

    private Animator animator;
    // hack tmp vars
    private Vector3 tmpOriginalScale;
    private float timer = 0.0f;
    private float squashDuration = 0.5f;  // default Duration seconds
    private float squashTargetY = 0.1f;  // default Target Y scale
    private bool is_squashed = false;  // flag Squash Active

    // constants  ==============================================================
    readonly private int IN_MOVEMENT_ID = Animator.StringToHash("InMovement");
    readonly private int JUMP_ID = Animator.StringToHash("Jump");

    // public methods  =========================================================

    public void AnimationDash() {
        Debug.Log("Dash");

        // hack need animation for dash
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
}