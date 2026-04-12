using UnityEngine;

[RequireComponent(typeof(FPSCounter))]

// TODO metrics: fps
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

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        // singleton logic  ----------------------------------------------------
        I = this;

        FPSCounter fpsCounter = GetComponent<FPSCounter>();
        if (fpsCounter == null) {
            Debug.LogWarning("fail to find: FPSCounter");
        } else {
            fpsCounter.OnFPSUpdate += OnFPSUpdate;

        }

    }



    // Event Handler  ##########################################################

    private void OnFPSUpdate(int fps) {
        // TODO
    }




#endif
}
