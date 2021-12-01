using System.IO;
using Models;
using UnityEngine;

namespace Managers
{
    public static class SettingsManager
    {
        private static readonly string SettingsFileName = $"{Application.persistentDataPath}/settings.json";
        private static GameSettings _settings;

        public static GameSettings Settings => _settings;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Inject()
        {
            if (File.Exists(SettingsFileName))
            {
                using (var str = new StreamReader(SettingsFileName))
                {
                    var json = str.ReadToEnd();
                    _settings = JsonUtility.FromJson<GameSettings>(json);
                }
            }
            else
            {
                _settings = new GameSettings();
            }
        }
    }
}