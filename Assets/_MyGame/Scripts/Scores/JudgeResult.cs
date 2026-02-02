using System;


[Flags]
public enum JudgeResult {

    // results  ----------------------------------------------------------------

    NO_HIT = 0,
    EARLY_MISS = 1 << 0,
    LATE_MISS = 1 << 1,
    EARLY_GOOD = 1 << 2,
    LATE_GOOD = 1 << 3,
    EARLY_GREAT = 1 << 4,
    LATE_GREAT = 1 << 5,
    EARLY_PERFECT = 1 << 6,
    LATE_PERFECT = 1 << 7,

    // groups  -----------------------------------------------------------------
    NO_SCORE = NO_HIT | EARLY_MISS | LATE_MISS,
    MISS = EARLY_MISS | LATE_MISS,
    GOOD = EARLY_GOOD | LATE_GOOD,
    GREAT = EARLY_GREAT | LATE_GREAT,
    PERFECT = EARLY_PERFECT | LATE_PERFECT
}