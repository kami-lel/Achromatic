using UnityEngine;
using System.Collections;


namespace Assets._Achromatic.Scripts.UI {
    public class FinalPointWindow: MonoBehaviour {

        // Inspector Fields  ###################################################
        [SerializeField] private float deactivateDelay = 6f;

        // MonoBehavior Lifecycle  #############################################

        private void OnEnable() {
            StartCoroutine(DeactivateAfterDelay());
        }

        // private methods  ####################################################
        //
        IEnumerator DeactivateAfterDelay() {
            yield return new WaitForSeconds(deactivateDelay);
            gameObject.SetActive(false);
        }
    }

}