using System;


namespace Assets._Achromatic.Scripts.Scores {
    [Flags]
    public enum Judgement {

        // results  ----------------------------------------------------------------

        NO_HIT = 1 << 0,
        EARLY_MISS = 1 << 1,
        LATE_MISS = 1 << 2,
        INCORRECT = 1 << 3,
        EARLY_GOOD = 1 << 4,
        LATE_GOOD = 1 << 5,
        EARLY_GREAT = 1 << 6,
        LATE_GREAT = 1 << 7,
        EARLY_PERFECT = 1 << 8,
        LATE_PERFECT = 1 << 9,

        // groups  -----------------------------------------------------------------
        NO_SCORE = NO_HIT | MISS | INCORRECT,
        EARLY = EARLY_MISS | EARLY_GOOD | EARLY_GREAT | EARLY_PERFECT,
        LATE = LATE_MISS | LATE_GOOD | LATE_GREAT | LATE_PERFECT,

        // score types
        MISS = EARLY_MISS | LATE_MISS,
        GOOD = EARLY_GOOD | LATE_GOOD,
        GREAT = EARLY_GREAT | LATE_GREAT,
        PERFECT = EARLY_PERFECT | LATE_PERFECT,
    }
}