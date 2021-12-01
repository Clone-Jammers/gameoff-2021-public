using System;

namespace Components.Logger
{
    public static class GameLogger
    {
        internal static event Action<string, LogTypes> onLog;

        public static void Log(string message, LogTypes types)
            => onLog?.Invoke(message, types);

        public static void LogDebug(string message)
            => onLog?.Invoke(message, LogTypes.Debug);
        
        public static void LogInfo(string message)
            => onLog?.Invoke(message, LogTypes.Info);

        public static void LogWarning(string message)
            => onLog?.Invoke(message, LogTypes.Warning);
        
        public static void LogError(string message)
            => onLog?.Invoke(message, LogTypes.Error);
    }
}