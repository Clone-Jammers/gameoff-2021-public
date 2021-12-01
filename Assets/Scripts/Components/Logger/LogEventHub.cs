using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Components.Logger
{
    [CreateAssetMenu(menuName = "Logger/Create Log Event Hub", fileName = "LogEventHub")]
    public class LogEventHub : ScriptableObject
    {
        public event Action<string, LogTypes> onLog;

        [Title("Log Flags")] [EnumToggleButtons] [SerializeField]
        private LogTypes _logTypes;

        private void OnEnable()
        {
            GameLogger.onLog += OnFilteredLog;
        }

        private void OnDisable()
        {
            GameLogger.onLog -= OnFilteredLog;
        }

        private void OnFilteredLog(string message, LogTypes type)
        {
            if (onLog != null && _logTypes.HasFlag(type))
            {
                onLog(message, type);
            }
        }
    }
}