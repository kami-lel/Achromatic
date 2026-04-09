using UnityEngine;


// Todo metrics save to file
// Todo local leaderboard

public class GameStats: MonoBehaviour {
    public static GameStats Instance {
        get; private set;
    }  // singleton Instance for Global Access

    public int maxCombo = 0;  // public Max Combo value
    public int totalScore = 0;  // public Total Score value

    void Awake() {
        if (Instance == null) {
            Instance = this;  // assign Singleton Instance
            DontDestroyOnLoad(gameObject);  // persist Across Scenes
        } else if (Instance != this) {
            Destroy(gameObject);  // prevent Duplicate Singletons
        }
    }
}