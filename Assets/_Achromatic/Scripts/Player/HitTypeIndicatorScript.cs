using Assets._Achromatic.Scripts.Scores;
using UnityEngine;

public class HitTypeIndicatorScript: MonoBehaviour {
    // todo animation for hit type indicator

    // public methods  =========================================================

    public void Show(Hit hit) {
        // set which symbol is active
        miss.SetActive((hit & Hit.NO_SCORE) != 0);
        earlyGood.SetActive(hit == Hit.EARLY_GOOD);
        lateGood.SetActive(hit == Hit.LATE_GOOD);
        earlyGreat.SetActive(hit == Hit.EARLY_GREAT);
        lateGreat.SetActive(hit == Hit.LATE_GREAT);
        perfect.SetActive((hit & Hit.PERFECT) != 0);
    }

    // Inspector Fields  =======================================================

    [SerializeField]
    private GameObject miss;

    [SerializeField]
    private GameObject earlyGood;

    [SerializeField]
    private GameObject lateGood;

    [SerializeField]
    private GameObject earlyGreat;

    [SerializeField]
    private GameObject lateGreat;

    [SerializeField]
    private GameObject perfect;

    // Inspector Fields  =======================================================

    void Start() {
        Show(Hit.NONE);
    }
}

