using UnityEngine;
using UnityEngine.InputSystem;

public class SFXMangerScript: MonoBehaviour {

    // todo randomize b/t different samples
    // Todo rumble control as its own script
    // Todo rumble fine tuning data
    // todo rumble to reflects both judge result & action type

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

    // todo audio cue to reflects both judge result & action type

    // public methods  ========================================================
    public void PlayJump() {
        sfxJump.Play();
        sfxJump.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
        PlayRumble(0.1f, 0.8f, 0.1f);
    }

    public void PlayDash() {
        sfxDash.Play();
        sfxDash.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
        PlayRumble(0.7f, 0.1f, 0.2f);
    }

    // private methods  ========================================================
    public void PlayRumble(float low, float high, float duration) {
        var pad = Gamepad.current;
        if (pad == null) {
            return;
        }

        pad.SetMotorSpeeds(low, high);  // start motors
        StartCoroutine(StopRumbleAfter(pad, duration));
    }

    private System.Collections.IEnumerator StopRumbleAfter(
            Gamepad pad, float duration) {
        yield return new WaitForSeconds(duration);
        if (pad != null)
            pad.SetMotorSpeeds(0f, 0f);  // stop motors
    }


}

