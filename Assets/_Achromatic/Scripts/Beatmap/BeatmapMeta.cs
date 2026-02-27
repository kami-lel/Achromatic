using UnityEngine;

[CreateAssetMenu(fileName = "BeatmapMeta", menuName = "Scriptable Objects/BeatmapMeta")]
public class BeatmapMeta: ScriptableObject {

    [SerializeField]
    public TextAsset file;

    [Header("Music")]

    public float tempo;

    public int beatPerBar;

    public int subdivisionPerBeat;

    public int preludeBarCount;

    [Header("Render")]

    public float horizontalSpeedPerBeat;

    public float barlineRenderDistance;

    public float noteRenderDistance;

    public float silenceSecondBeforeMainSong;

    [Header("Judge")]

    public float perfectDeltaSecond;

    public float greatDeltaSecond;

    public float goodDeltaSecond;
}
