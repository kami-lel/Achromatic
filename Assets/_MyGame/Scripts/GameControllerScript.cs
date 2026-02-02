using System;
using System.Collections;
using UnityEngine;

public class GameControllerScript: MonoBehaviour {

    // Inspector Fields  #######################################################
    [SerializeField]
    private GameObject player;

    // public members  #########################################################

    /// <summary>
    /// singleton instance of <c>GameControllerScript</c>
    /// </summary>
    public static GameControllerScript Instance;  // singleton

    [NonSerialized]
    public PlayerScript playerScript;

    // class method  ###########################################################
    /// <returns>singleton player</returns>
    public static GameObject GetPlayer() {
        if (Instance == null) {
            Debug.LogError("GameControllerScript: Instance is null");
        }

        return Instance.player;
    }


    // MonoBehavior Lifecycle  #################################################
    private void Awake() {
        // singleton single instance
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            // avoid duplicates
            Debug.LogError(
                    "GameControllerScript: Duplicate instance:"
                    + gameObject.name);
            Destroy(gameObject);
        }

        // reference to playerScript
        playerScript = player.GetComponent<PlayerScript>();
    }
}
