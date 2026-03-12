using UnityEngine;
using TMPro;


// HACK rm
public class TmpCanvasScript: MonoBehaviour {
    [SerializeField] private TextMeshProUGUI a;
    [SerializeField] private TextMeshProUGUI b;

    void Start() {
        if (a == null || b == null)
            return;  // guard Null Text Fields
        UpdateFromStats();  // initialize display
    }

    // update displayed values  --------------------------------------------------
    public void UpdateFromStats() {
        var stats = GameStats.Instance;  // cache GameStats Instance
        if (stats != null) {
            a.text = $"Max Combo: {stats.maxCombo}";  // set Max Combo text
            b.text = $"Total Score: {stats.totalScore}";  // set Total Score text
        } else {
            a.text = "Max Combo: 0";  // fallback when missing
            b.text = "Total Score: 0";  // fallback when missing
        }
    }
}