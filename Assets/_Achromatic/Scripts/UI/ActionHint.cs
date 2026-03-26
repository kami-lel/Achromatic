using Assets._Achromatic.Scripts.Scores;
using UnityEngine;

public class ActionHint: MonoBehaviour {

    // Public API  #############################################################

    public void Perish(Hit hit) {
        gameObject.SetActive(false);
        Debug.Log("action hint disabled:\t" + hit);  // HACK make prefab disappear
    }
}
