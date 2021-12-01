using Components.Logger;
using UnityEngine;

namespace Components.Sinks
{
    /// <summary>
    /// Test Sink
    /// </summary>
    public class UnityLoggerSink : Sink
    {
        protected override void onEvent(string message, LogTypes types)
        {
            if (types.HasFlag(LogTypes.Error))
            {
                Debug.LogError(message);
            }
            else if (types.HasFlag(LogTypes.Warning))
            {
                Debug.LogWarning(message);
            }
            else
            {
                Debug.Log(message);
            }
        }
    }
}