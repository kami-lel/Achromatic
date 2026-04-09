using UnityEngine;
using System.Collections;
using Assets._Achromatic.Scripts.Scores;
using TMPro;


// Fixme better total score windows
// Fixme use icons w/o arrows
namespace Assets._Achromatic.Scripts.UI {
    public class FinalPointWindow: MonoBehaviour {

        // Inspector Fields  ###################################################

        [SerializeField]
        private float deactivateDelay = 6f;

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

        private void OnEnable() {
            StartCoroutine(DeactivateAfterDelay());

            running.text = $"Point: {score.runningScore}";
            combo.text = $"Combo: {score.maxCombo}";
            int perfectCnt = score.hitCnt[Hit.EARLY_PERFECT] + score.hitCnt[Hit.LATE_PERFECT];
            perfect.text = $"{perfectCnt}";
            int greatCnt = score.hitCnt[Hit.EARLY_GREAT] + score.hitCnt[Hit.LATE_GREAT];
            great.text = $"{greatCnt}";
            int goodCnt = score.hitCnt[Hit.EARLY_GOOD] + score.hitCnt[Hit.LATE_GOOD];
            good.text = $"{goodCnt}";
            // Todo missing good count
        }

        // private methods  ####################################################
        //
        IEnumerator DeactivateAfterDelay() {
            yield return new WaitForSeconds(deactivateDelay);
            gameObject.SetActive(false);
        }
    }

}