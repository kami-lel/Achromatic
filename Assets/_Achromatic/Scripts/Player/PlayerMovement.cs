using UnityEngine;


public class PlayerMovement {

    // public methods  =========================================================

    public void TurnLeft() {
        moveDir = -1;
        EnsureFacing(-1);
        p.StartRun();
    }

    public void TurnRight() {
        moveDir = 1;
        EnsureFacing(1);
        p.StartRun();
    }

    public void Stop() {
        moveDir = 0;
        p.StopRun();
    }

    public void Jump() {
        if (!IsOnGround())
            return;

        p.playerRB.AddForce(Vector2.up * JUMP_FORCE, ForceMode2D.Impulse);

        p.Jump();
    }

    // MonoBehavior Lifecycle  =================================================

    public void Start() {
        p.playerRB.bodyType = RigidbodyType2D.Dynamic;
        p.playerRB.gravityScale = GRAVITY_SCALE;
        p.playerRB.freezeRotation = true;
        p.playerRB.linearDamping = 0.0f;
    }

    public void FixedUpdate() {

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

    // constructor  ============================================================
    public PlayerMovement(PlayerScript parent) {
        p = parent;

        p.playerRB = p.GetComponent<Rigidbody2D>();
        // TODO support all movements
    }

    // constants  ==============================================================

    readonly private float GRAVITY_SCALE = 1.0f;
    readonly private float JUMP_FORCE = 8.0f;
    readonly private float MAX_WALKING_SPEED = 10.0f;


    // private members  ========================================================
    // cached references
    private readonly PlayerScript p;


    private bool isFacingRight = true;
    private int moveDir = 0;

    // private methods  ========================================================

    public bool IsOnGround() {
        return p.playerRB.IsTouchingLayers(p.groundLayerMask);
    }

    private void EnsureFacing(int dir) {
        if (dir == 0)
            return;
        bool shouldFaceRight = dir > 0;
        if (shouldFaceRight != isFacingRight)
            Flip();
    }

    private void Flip() {
        isFacingRight = !isFacingRight;
        Vector3 s = transform.localScale;
        s.x = -s.x;
        transform.localScale = s;
    }
}