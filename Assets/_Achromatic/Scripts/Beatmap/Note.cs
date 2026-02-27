using System;


namespace Assets._Achromatic.Scripts.Beatmap {

    /// <summary>
    /// represent a single note w/i beatmap
    /// </summary>
    public class BeatmapNote {

        /// <summary>
        /// bar count, starting at 1
        /// </summary>
        public int Bar => jsonNote.bar;

        /// <summary>
        /// beat count w/i bar, start at 1
        /// </summary>
        public int Beat => jsonNote.beat;

        /// <summary>
        /// beat division count, start at 1
        /// </summary>
        public int Subbeat => jsonNote.subbeat;

        public BeatmapNoteType type;

        private readonly BeatmapData.JsonDataNote jsonNote;

        public BeatmapNote(
                BeatmapData.JsonDataNote jsonNote) {

            this.jsonNote = jsonNote;

            // convert string to enum type
            type = jsonNote.type switch {
                "jump" => BeatmapNoteType.JUMP,
                "dash" => BeatmapNoteType.DASH,
                _ => throw new InvalidOperationException(
                    $"BeatmapNote: bad note type: {jsonNote.type}")
            };
        }
    }
}



/* HACK rm
public class Beatmap {

    public BeatmapData beatmapData;
    public float beatPerBar;
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

        // load element prefabs
        prefabPool = new BeatmapPrefabsPool(
                GameControllerScript.Instance.transform);

        // set up vars
        beatPerBar = beatmapData.BeatPerBar;

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

    public float CalcXFromBeat(float beatCount) {
        return 0.0f;  // HACK
        // return origin.x + beatCount * data.BeatSpeed;
    }


    public float CalcCurrentXFromBeat() {
        return CalcXFromBeat(currentBeatCount);
    }


}
*/