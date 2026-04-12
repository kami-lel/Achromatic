using System;
using UnityEngine;


// TODO metrics: total time
// TODO metrics: portion of time
// TODO metrics: deltas
// TODO metrics: hit / miss ratio per part

// TODO metrics save to file

public class Metrics: MonoBehaviour {
#if UNITY_EDITOR || DEVELOPMENT_BUILD

    // Public Members  #########################################################

    // singleton
    public static Metrics I {
        get; private set;
    }


    // Public Members  #########################################################

    public void LogFPS(int fps) {
        // TODO
    }

    private void LogSequenceKeyPoint(String keyPoint) {
        // TODO
    }

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        // singleton logic  ----------------------------------------------------
        I = this;

    }



#endif
}
