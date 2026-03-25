using Assets._Achromatic.Scripts.Scores;
using UnityEngine;

public class ActionHint: MonoBehaviour {

    // Public API  #############################################################

    public void OnHit(Hit hit) {
        enabled = false;
        Debug.Log("action hint disabled");   // HACK
    }
}
