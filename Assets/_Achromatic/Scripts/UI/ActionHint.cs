using Assets._Achromatic.Scripts.Scores;
using UnityEngine;

public class ActionHint: MonoBehaviour {

    // TODO allow reset

    // Public API  #############################################################

    public void Perish(Hit hit) {
        // todo different behavior of action hint based on hit type
        perishTime = Time.time;
    }

    // Inspector Fields  #######################################################

    [SerializeField]
    private AnimationCurve transformVsTime;

    // MonoBehavior Lifecycle  #################################################
    private void Update() {
        if (perishTime < 0.0f) {
            return;
        }

        float size = transformVsTime.Evaluate(Time.time - perishTime);
        transform.localScale = new Vector2(size, size);

        // TODO set active false
    }

    // private members  ########################################################
    private float perishTime = -1f;
}
