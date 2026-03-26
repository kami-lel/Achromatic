using UnityEngine;
using System.Collections;
using Assets._Achromatic.Scripts.Scores;


namespace Assets._Achromatic.Scripts.UI {
    public class FinalPointWindow: MonoBehaviour {

        // Inspector Fields  ###################################################

        [SerializeField]
        private float deactivateDelay = 6f;

        [SerializeField]
        private Score score;

        // MonoBehavior Lifecycle  #############################################

        private void OnEnable() {
            StartCoroutine(DeactivateAfterDelay());

            Debug.Log(score.runningScore);  // HACK
        }

        // private methods  ####################################################
        //
        IEnumerator DeactivateAfterDelay() {
            yield return new WaitForSeconds(deactivateDelay);
            gameObject.SetActive(false);
        }
    }

}