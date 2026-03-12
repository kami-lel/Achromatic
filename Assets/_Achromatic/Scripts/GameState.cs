
using System;

[Flags]
public enum GameState {
    NONE = 0,

    EXPLORE = 1 << 0,
    VAMP = 1 << 1,
    PRELUDE = 1 << 2,
    MAIN_PIECE = 1 << 3,
    PIECE_FINISHED = 1 << 4,
    SCENE_TRANSITION = 1 << 5,

    EXPLORE_CONTROL = EXPLORE | VAMP,
    PIECE_CONTROl = PRELUDE | MAIN_PIECE

}
