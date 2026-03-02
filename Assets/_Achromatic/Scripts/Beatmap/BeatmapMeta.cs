using UnityEngine;

[CreateAssetMenu(fileName = "BeatmapMeta", menuName = "Scriptable Objects/BeatmapMeta")]
public class BeatmapMeta: ScriptableObject {

    [SerializeField]
    public TextAsset file;

    [Header("Music")]

    public float tempo = 120;

    public int beatPerBar = 4;

    public int subdivisionPerBeat = 4;

    public int preludeBarCount = 8;

    [Header("Render")]

    public float horizontalSpeedPerBeat = 2.0f;

    public float barlineRenderDistance = 10.0f;

    public float noteRenderDistance = 10.0f;

    public float silenceSecondBeforeMainSong = 1.0f;

    [Header("Judge")]

    public float perfectDeltaSecond = 0.05f;

    public float greatDeltaSecond = 0.1f;

    public float goodDeltaSecond = 0.3f;
}
