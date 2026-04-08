using UnityEngine;

public class GameController: MonoBehaviour {

    // Public Members  #########################################################

    // singleton
    public static GameController I {
        get; private set;
    }

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        // singleton logic  ----------------------------------------------------
        if (I != null && I != this) {
            Debug.LogWarning("duplicated GameController", this);
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);
    }

}
