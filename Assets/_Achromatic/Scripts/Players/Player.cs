using System;
using UnityEngine;

namespace Assets._Achromatic.Scripts.Players {

    public class Player: MonoBehaviour {

        // Public Members  #####################################################
        [NonSerialized]
        public Rigidbody2D rb;

        public event Action<string> OnTriggerEnter;
        public event Action<string> OnTriggerExit;

        // MonoBehavior Lifecycle  #############################################

        private void Start() {
            GCS.I.states = GameState.EXPLORE;
        }

        // event handlers  #####################################################

        private void OnTriggerEnter2D(Collider2D other) {
            if ((GCS.I.states & GameState.EXPLORE_CONTROL) == 0 ||
                    other == null || !other.isTrigger) {
                return;
            }

            OnTriggerEnter?.Invoke(other.tag);
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other == null || !other.isTrigger) {
                return;
            }

            OnTriggerExit?.Invoke(other.tag);
        }
    }
}
