using UnityEngine;
using System.Collections;
using Assets._Achromatic.Scripts.Scores;
using TMPro;


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

            running.text = $"{score.runningScore}";
        }

        // private methods  ####################################################
        //
        IEnumerator DeactivateAfterDelay() {
            yield return new WaitForSeconds(deactivateDelay);
            gameObject.SetActive(false);
        }
    }

}