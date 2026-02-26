
using System.Collections.Generic;

using UnityEngine;

using Assets._Achromatic.Scripts.Beatmap;

// FIXME organize & factorization

public class Beatmap {

    public BeatmapData beatmapData;
    public float beatPerBar;
    readonly private float tempoDiv60;
    readonly private float preludeOffsetAsBeat;
    readonly private BeatmapPrefabsPool prefabPool;
    private float lastBeatLineOnBeat;
    private float lastBarlineOnBeat;
    private Vector2 origin;

    // constants  ##############################################################

    /// <summary>
    /// height of note on board
    /// </summary>
    private const float NOTES_HEIGHT = 2.5f; // Fixme more dynamic?

    /// <summary>
    /// current beat count, <c>0.0f</c> at start,
    /// consistent in the same <c>Update()</c>
    /// </summary>
    public float currentBeatCount;

    /// <summary>
    /// local dynamic copy used for render
    /// </summary>
    private Queue<BeatmapNote> notesRenderQ;

    public Beatmap(Vector2 origin, TextAsset beatmapFile) {
        this.origin = origin;

        // load & set up beatmap
        if (beatmapFile == null) {
            Debug.LogWarning("PieceScript: must provide beatmapFile");
        }

        beatmapData = new BeatmapData(beatmapFile);

        // load element prefabs
        prefabPool = new BeatmapPrefabsPool(
                GameControllerScript.Instance.transform);

        // set up vars
        beatPerBar = beatmapData.BeatPerBar;
        tempoDiv60 = beatmapData.Tempo / 60.0f;
        preludeOffsetAsBeat = beatmapData.PreludeLength * tempoDiv60;

        lastBeatLineOnBeat = 0.0f;
        lastBarlineOnBeat = 0.0f;
        notesRenderQ = new(beatmapData.notes);
    }

    public void Update() {
        // todo make note disappear / animation when hit
        // place beatLine  -----------------------------------------------------
        float renderBoundaryOnBeat = currentBeatCount
                + beatmapData.BarlineRenderDistance * beatPerBar;
        while (renderBoundaryOnBeat - lastBeatLineOnBeat > 1.0f) {
            float placeOnBeat = lastBeatLineOnBeat + 1.0f;

            prefabPool.Spawn("BeatLine",
                    new Vector2(CalcXFromBeat(placeOnBeat), 0.0f));

            lastBeatLineOnBeat = placeOnBeat;
        }

        // place barline  ------------------------------------------------------
        renderBoundaryOnBeat = currentBeatCount + beatmapData.BarlineRenderDistance;
        while (renderBoundaryOnBeat - lastBarlineOnBeat > beatPerBar) {
            float placeOnBeat = lastBarlineOnBeat + beatPerBar;

            prefabPool.Spawn("Barline",
                    new Vector2(CalcXFromBeat(placeOnBeat), 0.0f));

            lastBarlineOnBeat = placeOnBeat;
        }

        // Fixme barline placement overlaps beat lines
        // Bug 1st barline missing

        // render notes  -------------------------------------------------------
        renderBoundaryOnBeat = currentBeatCount + beatmapData.NoteRenderDistance;

        while (notesRenderQ.Count > 0) {
            var next = notesRenderQ.Peek();
            float noteOnBeat = next.CalcBeatCount();

            if (noteOnBeat >= renderBoundaryOnBeat)
                break;

            // place the note
            BeatmapNote note = notesRenderQ.Dequeue();

            string prefabName = note.type switch {
                BeatmapNoteType.JUMP => "JumpNote",
                BeatmapNoteType.DASH => "DashNote",
                _ => null
            };

            prefabPool.Spawn(prefabName,
                    new Vector2(CalcXFromBeat(noteOnBeat), NOTES_HEIGHT));

        }

    }

    /// <returns>realtime beat count based on Audio Source time,
    /// start on <c>0.0f</c></returns>
    public float CalcRealtimeBeatCount(AudioSource audioSource) {
        return audioSource.time * tempoDiv60 - preludeOffsetAsBeat;
    }

    public float CalcXFromBeat(float beatCount) {
        return origin.x + beatCount * beatmapData.BeatSpeed;
    }

    public float CalcCurrentXFromBeat() {
        return CalcXFromBeat(currentBeatCount);
    }

}