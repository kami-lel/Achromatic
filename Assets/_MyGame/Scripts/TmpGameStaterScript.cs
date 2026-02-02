using UnityEngine;

// hack rm this class
public class TmpGameStaterScript: MonoBehaviour {

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject piece;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == player && !piece.activeSelf) {
            piece.SetActive(true);
        }
    }
}
