
using System;


namespace Assets._Achromatic.Scripts.Players {

    [Flags]
    public enum Actions {
        NONE = 0,
        JUMP = 1 << 0,
        SQUAT = 1 << 1,
        ATTACK = 1 << 2,
    }
}