using UnityEngine;

[CreateAssetMenu(fileName = "BeatmapMeta", menuName = "Scriptable Objects/BeatmapMeta")]
public class BeatmapMeta: ScriptableObject {

    [SerializeField]
    public TextAsset file;

    [Header("Music")]

    public float tempo = 120;

    public int beatPerBar = 4;

    public int subdivisionPerBeat = 4;

    public float preludeSeconds = 1.0f;

    [Header("Render")]
    public float elementsSpeedMultiplier = 0.0f;

    public float horizontalUnitsPerBeat = 2.0f;

    public float barlineRenderDistance = 10.0f;

    public float noteRenderDistance = 10.0f;

    [Header("Judge")]

    public float perfectDeltaSecond = 0.05f;

    public float greatDeltaSecond = 0.1f;

    public float goodDeltaSecond = 0.3f;
}
