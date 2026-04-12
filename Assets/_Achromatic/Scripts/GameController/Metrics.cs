using UnityEngine;

// TODO metrics: fps,total time &portion of time,deltas,hit / miss ratio per part

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
