using UnityEngine;

namespace Assets._Achromatic.Scripts.Scores {
    public class HitTypeIndicatorScript: MonoBehaviour {
        // TODO animation for hit type indicator

        // Public Methods  #####################################################

        public void Show(Hit hit) {
            // set which symbol is active
            miss.SetActive((hit & Hit.NO_SCORE) != 0);
            earlyGood.SetActive(hit == Hit.EARLY_GOOD);
            lateGood.SetActive(hit == Hit.LATE_GOOD);
            earlyGreat.SetActive(hit == Hit.EARLY_GREAT);
            lateGreat.SetActive(hit == Hit.LATE_GREAT);
            perfect.SetActive((hit & Hit.PERFECT) != 0);
        }

        // Inspector Fields  ###################################################

        [SerializeField]
        private GameObject miss;

        [SerializeField]
        private GameObject earlyGood;

        [SerializeField]
        private GameObject lateGood;

        [SerializeField]
        private GameObject earlyGreat;

        [SerializeField]
        private GameObject lateGreat;

        [SerializeField]
        private GameObject perfect;

        // MonoBehavior Lifecycle  #############################################
        private void Start() {
            Show(Hit.NONE);
        }
    }
}