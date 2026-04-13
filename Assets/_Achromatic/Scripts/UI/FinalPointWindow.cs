using Assets._Achromatic.Scripts.Scores;
using TMPro;
using UnityEngine;


// FIXME better total score windows
namespace Assets._Achromatic.Scripts.UI {
    public class FinalPointWindow: MonoBehaviour {
        // Inspector Fields  ###################################################

        [SerializeField]
        private Score score;

        [SerializeField]
        private TextMeshProUGUI combo;

        [SerializeField]
        private TextMeshProUGUI running;

        [SerializeField]
        private TextMeshProUGUI perfect;

        [SerializeField]
        private TextMeshProUGUI great;

        [SerializeField]
        private TextMeshProUGUI good;

        [SerializeField]
        private TextMeshProUGUI miss;

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            if (score == null) {
                Debug.LogWarning("must assign: score", this);
            }
        }


        private void OnEnable() {
            // closing conditions  ---------------------------------------------

            running.text = $"Point: {score.runningScore}";
            combo.text = $"Combo: {score.maxCombo}";
            int perfectCnt =
                score.hitCnt[Hit.EARLY_PERFECT]
                + score.hitCnt[Hit.LATE_PERFECT];
            perfect.text = $"{perfectCnt}";
            int greatCnt =
                score.hitCnt[Hit.EARLY_GREAT] + score.hitCnt[Hit.LATE_GREAT];
            great.text = $"{greatCnt}";
            int goodCnt =
                score.hitCnt[Hit.EARLY_GOOD] + score.hitCnt[Hit.LATE_GOOD];
            good.text = $"{goodCnt}";
            // TODO missing good count
        }
    }
}
