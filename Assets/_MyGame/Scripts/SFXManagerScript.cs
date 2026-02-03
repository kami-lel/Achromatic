using UnityEngine;

public class SFXMangerScript: MonoBehaviour {

    // todo randomize b/t different samples
    // TODO send feedback to controller

    // Inspector Fields  =======================================================
    [SerializeField]
    private AudioSource sfxJump;

    [SerializeField]
    private AudioSource sfxDash;

    // public members  =========================================================
    /// <summary>
    /// singleton instance of <c>GameControllerScript</c>
    /// </summary>
    public static SFXMangerScript Instance;  // singleton

    // MonoBehavior Lifecycle  =================================================
    private void Awake() {
        // singleton single instance
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            // avoid duplicates
            Debug.LogError(
                    "AudioControllerScript: Duplicate instance:"
                    + gameObject.name);
            Destroy(gameObject);
        }
    }


    // public methods  ========================================================
    public void PlayJump() {
        sfxJump.Play();
        sfxJump.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
    }

    public void PlayDash() {
        sfxDash.Play();
        sfxDash.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
    }

}

