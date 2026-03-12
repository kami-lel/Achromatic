using Assets._Achromatic.Scripts.Scores;
using UnityEngine;
using UnityEngine.InputSystem;


public class SFXManagerScript: MonoBehaviour {

    // todo randomize b/t different samples
    // TODO rumble fine tuning data
    // TODO rumble to reflects both judge result & action type
    // todo audio cue to reflects both judge result & action type

    // public members  =========================================================

    // singleton
    public static SFXManagerScript I {
        get; private set;
    }

    // public methods  =========================================================

    public void Jump(Hit hit = Hit.NONE) {
        jumpSFX.Play();
        jumpSFX.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
        PlayRumble(0.1f, 0.8f, 0.1f);
    }

    public void Squat(Hit hit = Hit.NONE) {
        dashSFX.Play();
        dashSFX.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
        PlayRumble(0.7f, 0.1f, 0.2f);
    }

    public void Attack(Hit hit = Hit.NONE) {
        dashSFX.Play();
        dashSFX.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
        PlayRumble(0.7f, 0.1f, 0.2f);
    }


    // Inspector Fields  =======================================================
    [SerializeField]
    private AudioSource jumpSFX;

    [SerializeField]
    private AudioSource dashSFX;

    // MonoBehavior Lifecycle  =================================================
    private void Awake() {
        if (I == null) {  // create Singleton
            I = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        if (I != this) {  // guard against duplicate
            Debug.LogError("SFX:\tplace SFXManager Prefab only in 1st scene");
            Destroy(gameObject);
        }
    }

    // controller rumbling  ====================================================

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

        pad?.SetMotorSpeeds(0f, 0f);   // stop motors
    }
}
