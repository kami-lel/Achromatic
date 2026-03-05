using System;


namespace Assets._Achromatic.Scripts.Pieces {

    [Flags]
    public enum PressedActions {
        NONE = 0,
        JUMP = 1 << 0,
        DASH = 1 << 1,
        POWER_JUMP = 1 << 2,
    }

}