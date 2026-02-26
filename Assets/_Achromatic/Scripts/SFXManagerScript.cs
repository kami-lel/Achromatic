using UnityEngine;
using UnityEngine.InputSystem;

public class SFXManagerScript: MonoBehaviour {

    // Todo randomize b/t different samples
    // TODO rumble control as its own script
    // Todo rumble fine tuning data
    // Todo rumble to reflects both judge result & action type
    // Todo audio cue to reflects both judge result & action type

    // Inspector Fields  #######################################################
    [SerializeField]
    private AudioSource jumpSFX;

    [SerializeField]
    private AudioSource dashSFX;

    // MonoBehavior Lifecycle  #################################################
    private void Awake() {
        Instance = this;
    }

    // public member  ##########################################################
    // singleton instance
    public static SFXManagerScript Instance;


    // public methods  #########################################################

    public void PlayJumpSFX() {
        jumpSFX.Play();
        jumpSFX.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
        PlayRumble(0.1f, 0.8f, 0.1f);
    }

    public void PlayDashSFX() {
        dashSFX.Play();
        dashSFX.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
        PlayRumble(0.7f, 0.1f, 0.2f);
    }

    // private methods  ########################################################

    private void PlayRumble(float low, float high, float duration) {
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
