using UnityEngine;

public class Metrics: MonoBehaviour {

#if UNITY_EDITOR || DEVELOPMENT_BUILD

    // Public Members  #########################################################

    // singleton
    public static Metrics I {
        get; private set;
    }

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        // singleton logic  ----------------------------------------------------
        I = this;
    }


#endif
}
