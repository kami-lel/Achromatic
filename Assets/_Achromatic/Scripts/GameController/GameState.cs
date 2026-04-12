
using System;

[Flags]
public enum GameState {
    NONE = 0,

    EXPLORE = 1 << 0,
    VAMP = 1 << 1,
    PRELUDE1 = 1 << 2,
    PRELUDE2 = 1 << 3,
    MAIN_PIECE = 1 << 4,
    TOTAL_SCORE_WINDOW = 1 << 5,
    SCENE_TRANSITION = 1 << 6,

    PRELUDE = PRELUDE1 | PRELUDE2,

    EXPLORE_CONTROL = EXPLORE | VAMP,
    PIECE_CONTROl = PRELUDE | MAIN_PIECE
}
