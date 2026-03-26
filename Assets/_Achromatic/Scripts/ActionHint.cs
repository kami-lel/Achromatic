using Assets._Achromatic.Scripts.Scores;
using UnityEngine;

public class ActionHint: MonoBehaviour {

    // Public API  #############################################################

    public void OnHit(Hit hit) {
        gameObject.SetActive(false);
        Debug.Log("action hint disabled");  // Hack make prefab disappear
    }
}
