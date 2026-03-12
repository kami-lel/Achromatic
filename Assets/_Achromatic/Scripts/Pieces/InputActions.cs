using System;


namespace Assets._Achromatic.Scripts.Pieces {

    [Flags]
    public enum PressedActions {
        NONE = 0,
        JUMP = 1 << 0,
        SQUAT = 1 << 1,
        ATTACK = 1 << 2,
    }

}