using UnityEngine;

[CreateAssetMenu(fileName = "BeatmapSetting", menuName = "Scriptable Objects/BeatmapSetting")]
public class BeatmapSetting: ScriptableObject {

    [Header("Music")]
    public float tempo;
    public int beatPerBar;
    public int subdivisionPerBeat;
    public int preludeBarCount;

    [Header("Render")]
    public float horizontalSpeedPerBeat;
    public float barlineRenderDistance;
    public float noteRenderDistance;

    [Header("Judge")]
    public float perfectDeltaSecond;
    public float greatDeltaSecond;
    public float goodDeltaSecond;
}
