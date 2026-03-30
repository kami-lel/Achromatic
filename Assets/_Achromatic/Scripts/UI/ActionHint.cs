using Assets._Achromatic.Scripts.Scores;
using UnityEngine;

public class ActionHint: MonoBehaviour {

    // Public API  #############################################################

    public void Perish(Hit hit) {
        // Todo different behavior of action hint based on hit type
        // Todo larger visual difference before/after perishing
        perishTime = Time.time;
    }

    // Inspector Fields  #######################################################

    [SerializeField]
    private AnimationCurve transformVsTime;

    // MonoBehavior Lifecycle  #################################################

    private void OnEnable() {
        perishTime = -1f;
        transform.localScale = new Vector2(1.0f, 1.0f);
    }

    private void Update() {
        if (perishTime < 0.0f) {
            return;
        }

        float size = transformVsTime.Evaluate(Time.time - perishTime);
        transform.localScale = new Vector2(size, size);
    }

    // private members  ########################################################
    private float perishTime;
}
