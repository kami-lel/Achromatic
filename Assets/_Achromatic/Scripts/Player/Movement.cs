using UnityEngine;


namespace Assets._Achromatic.Scripts.Player {

    class Movement: MonoBehaviour {

        // Public API  #########################################################
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

            SFX.I.Jump();
            p.Jump();
        }

        public void Squat() {
            if (!IsOnGround())
                return;

            p.Squat();
            SFX.I.Squat();
        }

        // MonoBehavior Lifecycle  #############################################
        private void Start() {
            p.playerRB.bodyType = RigidbodyType2D.Dynamic;
            p.playerRB.gravityScale = GRAVITY_SCALE;
            p.playerRB.freezeRotation = true;
            p.playerRB.linearDamping = 0.0f;

        }

        private void FixedUpdate() {
            // apply horizontal force toward target velocity
            float targetVelX = moveDir * MAX_WALKING_SPEED;
            float velDiff = targetVelX - p.playerRB.linearVelocityX;
            float requiredAccel = velDiff / Time.fixedDeltaTime;
            float maxForce = Mathf.Abs(requiredAccel * p.playerRB.mass);
            // clamp force to avoid extreme impulses
            float forceX = Mathf.Clamp(requiredAccel * p.playerRB.mass,
                -maxForce, maxForce);
            p.playerRB.AddForce(new Vector2(forceX, 0f));

            // light damping when idle to reduce sliding
            if (moveDir == 0 && Mathf.Abs(p.playerRB.linearVelocityX) < 0.01f) {
                p.playerRB.linearVelocity = new Vector2(0f, p.playerRB.linearVelocityY);
            }
        }



        // constants  ##########################################################
        private static readonly float GRAVITY_SCALE = 1.0f;
        private static readonly float JUMP_FORCE = 8.0f;
        private static readonly float MAX_WALKING_SPEED = 10.0f;

        // private members  ####################################################
        private bool isFacingRight = true;
        private int moveDir = 0;

        // private methods  ########################################################

        private bool IsOnGround() {
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
            Vector3 s = p.transform.localScale;
            s.x = -s.x;
            p.transform.localScale = s;
        }
        // TODO

    }
}
