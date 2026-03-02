using UnityEngine;

// HACK use the real audio plugin

public class PseudoAudioPlugin: MonoBehaviour {
    [SerializeField]
    public AudioSource bgm;

    [SerializeField]
    public AudioSource vamp;

    [SerializeField]
    public AudioSource preludeAndMain;
}
