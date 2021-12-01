using System;

namespace Components.Logger
{
    [Flags]
    public enum LogTypes : short
    {
        InGame = 1 << 1,
        Debug = 1 << 2,
        Info = 1 << 3,
        Warning = 1 << 4,
        Error = 1 << 5
    }
}