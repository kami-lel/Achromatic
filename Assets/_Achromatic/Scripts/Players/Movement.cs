using System;
using UnityEngine;


namespace Assets._Achromatic.Scripts.Players {
    [RequireComponent(typeof(AnimationManager))]
    [RequireComponent(typeof(Rigidbody2D))]
    class Movement: MonoBehaviour {

        // Public API  #########################################################
        public void TurnLeft() {
            moveDir = -1;
            anim.EnsureFacing(false);
            anim.StartRun();
        }

        public void TurnRight() {
            moveDir = 1;
            anim.EnsureFacing(true);
            anim.StartRun();
        }

        public void Stop() {
            moveDir = 0;
            anim.StopRun();
        }

        public void Jump() {
            if (!IsOnGround())
                return;

            rb.AddForce(Vector2.up * JUMP_FORCE, ForceMode2D.Impulse);

            SFX.I.Jump();
            anim.Jump();
        }

        public void Squat() {
            if (!IsOnGround())
                return;

            anim.Squat();
            SFX.I.Squat();
        }

        // Inspector Fields  ###################################################

        [SerializeField]
        private LayerMask groundLayerMask = Physics2D.AllLayers;

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            anim = GetComponent<AnimationManager>();
            if (anim == null) {
                Debug.LogError("Movement:\tfail to get: AnimationManager");
            }

            rb = GetComponent<Rigidbody2D>();
            if (rb == null) {
                Debug.LogError("Movement:\tfail to get: Rigidbody2D");
            }

        }

        private void Start() {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = GRAVITY_SCALE;
            rb.freezeRotation = true;
            rb.linearDamping = 0.0f;
        }
        private void FixedUpdate() {
            if ((GCS.I.states & GameState.EXPLORE_CONTROL) == 0) {
                return;
            }

            // apply horizontal force toward target velocity
            float targetVelX = moveDir * MAX_WALKING_SPEED;
            float velDiff = targetVelX - rb.linearVelocityX;
            float requiredAccel = velDiff / Time.fixedDeltaTime;
            float maxForce = Mathf.Abs(requiredAccel * rb.mass);
            // clamp force to avoid extreme impulses
            float forceX = Mathf.Clamp(requiredAccel * rb.mass,
                -maxForce, maxForce);
            rb.AddForce(new Vector2(forceX, 0f));

            // light damping when idle to reduce sliding
            if (moveDir == 0 && Mathf.Abs(rb.linearVelocityX) < 0.01f) {
                rb.linearVelocity = new Vector2(0f, rb.linearVelocityY);
            }
        }

        // constants  ##########################################################
        private static readonly float GRAVITY_SCALE = 1.0f;
        private static readonly float JUMP_FORCE = 8.0f;
        private static readonly float MAX_WALKING_SPEED = 10.0f;

        // private members  ####################################################
        private int moveDir = 0;

        // Cached References
        private AnimationManager anim;
        private Rigidbody2D rb;

        // private methods  ####################################################

        private bool IsOnGround() {
            return rb.IsTouchingLayers(groundLayerMask);
        }

    }
}
