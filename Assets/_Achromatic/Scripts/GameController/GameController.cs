using UnityEngine;

public class GameController: MonoBehaviour {

    // Public Members  #########################################################

    // singleton
    public static GameController I {
        get; private set;
    }

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        if (I == null) {
        } else {
            Debug.LogWarning("GameController singleton replaced", this);
        }

        I = this;
        DontDestroyOnLoad(gameObject);
    }

}
