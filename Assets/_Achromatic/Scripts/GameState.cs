
using System;

[Flags]
public enum GameState {
    NONE = 0,

    EXPLORE = 1 << 0,
    PRELUDE = 1 << 1,
    MUSIC_PLAY = 1 << 2,

}
