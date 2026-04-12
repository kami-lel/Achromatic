using UnityEngine;

public class Statistics: MonoBehaviour {
    // Public Members  #########################################################

    // singleton
    public static Statistics I {
        get; private set;
    }

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        I = this;
    }

}