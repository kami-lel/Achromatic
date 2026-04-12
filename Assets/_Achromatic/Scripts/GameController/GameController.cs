using UnityEngine;

public class GameController: MonoBehaviour {
    // Public Members  #########################################################

    // singleton
    public static GameController I {
        get; private set;
    }

    public GameObject FindMainPlayer() {
        GameObject playerObject = GameObject.FindWithTag(PLAYER_TAG);

        if (playerObject == null) {
            Debug.LogError(
                $"GCS:\tfail to find GameObject with tag: {PLAYER_TAG}"
            );
        }

        return playerObject;
    }

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        // singleton logic  ----------------------------------------------------
        if (I != null && I != this) {
            Debug.LogWarning("duplicated GameController", this);
            Destroy(this);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);
    }

    // constants  ##############################################################
    private const string PLAYER_TAG = "Player";
}
