using UnityEngine;

// TODO mpl pseudoAudioPlugin

public class PseudoAudioPlugin: MonoBehaviour {
    [SerializeField]
    public AudioSource explore1;

    [SerializeField]
    public AudioSource explore2;

    [SerializeField]
    public AudioSource mainPiece;

    public void StartMap() {
        Debug.LogWarning("pseudoAudioPlugin: Start Map");

    }

    public void StartPrelude() {
        Debug.LogWarning("pseudoAudioPlugin: Start Prelude");

    }

    public void StartMainPiece() {
        Debug.LogWarning("pseudoAudioPlugin: Start Main Piece");
    }
}
