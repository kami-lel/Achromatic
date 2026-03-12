

using System;
using UnityEngine;

namespace Assets._Achromatic.Scripts.Players {

    class AnimationManager: MonoBehaviour {

        // Public API  #############################################################

        public void StartRun() {
            animator.SetBool(RUN_ANIM_ID, true);
        }

        public void StopRun() {
            animator.SetBool(RUN_ANIM_ID, false);
        }

        public void Jump() {
            animator.SetTrigger(JUMP_ANIM_ID);
        }

        public void Squat() {
            animator.SetTrigger(SQUAT_ANIM_ID);
        }

        public void Attack() {
            animator.SetTrigger(ATTACK_ANIM_ID);
        }

        // MonoBehavior Lifecycle  #############################################
        private void Awake() {
            animator = GetComponent<Animator>();
        }


        // constants  ##########################################################
        private readonly int RUN_ANIM_ID = Animator.StringToHash("Run");
        private readonly int ATTACK_ANIM_ID = Animator.StringToHash("Attack");
        private readonly int SQUAT_ANIM_ID = Animator.StringToHash("Squat");
        private readonly int JUMP_ANIM_ID = Animator.StringToHash("Jump");

        // private members  ########################################################
        private Animator animator;
    }
}
