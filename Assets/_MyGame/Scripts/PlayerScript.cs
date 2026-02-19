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

    // MonoBehavior Lifecycle  #################################################

    void Awake() {
        playerRB = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        SetPlayTypeAsMusic(false);
        playerInput.defaultActionMap = "PlayerExplorePlay";
        playerInput.onActionTriggered += OnActionTriggered;
    }

    void FixedUpdate() {
        MovementFixedUpdate();
    }

    private void Update() {
        AnimationUpdate();
    }

    private void OnDisable() {
        playerInput.onActionTriggered -= OnActionTriggered;
    }

    // public methods  #########################################################

    public void SetPlayTypeAsMusic(bool as_music_play) {
        this.as_music_play = as_music_play;

        if (as_music_play) {
            playerRB.bodyType = RigidbodyType2D.Kinematic;
            playerInput.SwitchCurrentActionMap("PlayerMusicPlay");

        } else {
            playerRB.bodyType = RigidbodyType2D.Dynamic;
            playerRB.gravityScale = GRAVITY_SCALE;
            playerRB.freezeRotation = true;

            playerInput.SwitchCurrentActionMap("PlayerExplorePlay");
        }

    }

    // private members  ########################################################

    private bool as_music_play;

    // inputs  #################################################################

    private PlayerInput playerInput;

    private void OnActionTriggered(InputAction.CallbackContext ctxt) {

        switch (ctxt.action.phase) {
        case InputActionPhase.Started:  // -------------------------------------
            switch (ctxt.action.name) {
            case "Jump":
                Jump();
                break;

            case "Left":
                TurnLeft();
                break;

            case "Right":
                TurnRight();
                break;

            case "Dash":
                Debug.Log("Dash");  // todo
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
        animator.SetBool(IN_MOVEMENT_ID, true);
    }

    public void TurnRight() {
        moveDir = 1;
        MovementEnsureFacing(1);
        animator.SetBool(IN_MOVEMENT_ID, true);
    }

    public void StopMovement() {
        moveDir = 0;
        animator.SetBool(IN_MOVEMENT_ID, false);
    }

    public void Jump() {
        if (!playerRB.IsTouchingLayers(groundLayerMask))
            return;

        playerRB.AddForce(Vector2.up * JUMP_FORCE, ForceMode2D.Impulse);

        animator.SetTrigger(JUMP_ID);
    }

    // private members  ========================================================

    private Rigidbody2D playerRB;
    bool isFacingRight = true;
    int moveDir = 0;

    // constants  --------------------------------------------------------------
    readonly private float GRAVITY_SCALE = 1.0f;
    readonly private float JUMP_FORCE = 5.0f;
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

    private void AnimationUpdate() {
        if (playerRB.IsTouchingLayers(groundLayerMask)) {
            // HACK
            // animator.SetBool(IN_AIR_ID, false);
        }
    }

    // constants  ==============================================================
    readonly private int IN_MOVEMENT_ID = Animator.StringToHash("InMovement");
    readonly private int JUMP_ID = Animator.StringToHash("Jump");


    // TODO SFX
}