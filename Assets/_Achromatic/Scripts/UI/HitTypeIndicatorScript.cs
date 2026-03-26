using UnityEngine;


using Assets._Achromatic.Scripts.Scores;

namespace Assets._Achromatic.Scripts.UI {
    public class HitTypeIndicatorScript: MonoBehaviour {

        // Public Methods  #####################################################

        public void Show(Hit hit) {
            // set which symbol is active
            miss.SetActive((hit & Hit.NO_SCORE) != 0);
            earlyGood.SetActive(hit == Hit.EARLY_GOOD);
            lateGood.SetActive(hit == Hit.LATE_GOOD);
            earlyGreat.SetActive(hit == Hit.EARLY_GREAT);
            lateGreat.SetActive(hit == Hit.LATE_GREAT);
            perfect.SetActive((hit & Hit.PERFECT) != 0);

            lastShowTime = Time.time;
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

        [SerializeField]
        private AnimationCurve transformVsTime;

        // MonoBehavior Lifecycle  #############################################
        private void Start() {
            Show(Hit.NONE);
        }

        private void Update() {
            float timeDelta = Time.time - lastShowTime;

            float size = transformVsTime.Evaluate(timeDelta);
            transform.localScale = new Vector2(size, size);
        }

        // private members  ####################################################
        private float lastShowTime;
    }
}