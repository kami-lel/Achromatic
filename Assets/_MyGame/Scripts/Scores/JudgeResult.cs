using System;


[Flags]
public enum JudgeResult {

    // results  ----------------------------------------------------------------

    NO_HIT = 0,
    EARLY_MISS = 1 << 0,
    LATE_MISS = 1 << 1,
    INCORRECT = 1 << 2,
    EARLY_GOOD = 1 << 3,
    LATE_GOOD = 1 << 4,
    EARLY_GREAT = 1 << 5,
    LATE_GREAT = 1 << 6,
    EARLY_PERFECT = 1 << 7,
    LATE_PERFECT = 1 << 8,

    // groups  -----------------------------------------------------------------
    NO_SCORE = NO_HIT | EARLY_MISS | LATE_MISS | INCORRECT,
    MISS = EARLY_MISS | LATE_MISS,
    GOOD = EARLY_GOOD | LATE_GOOD,
    GREAT = EARLY_GREAT | LATE_GREAT,
    PERFECT = EARLY_PERFECT | LATE_PERFECT
}