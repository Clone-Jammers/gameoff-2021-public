using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Components.Logger;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Components.Sinks
{
    public class Spammer : MonoBehaviour
    {
        [SerializeField] [MinMaxSlider(0, 600, true)]
        private Vector2 timeSpan;

        static string ErrorCodeGenerator()
        {
            var number = Random.Range(1, 700);
            return $"0x{number.ToString("X").PadLeft(22, '0')}";
        }

        private static readonly Dictionary<int, string> _logMessages = new()
        {
            { 0, "AI spawn position created." },
            { 1, "Next GC scheduled." },
            { 2, "Unloading previous section." },
            { 3, "Enemy pool cycled." },
            { 4, "Rolling new SPM interop messages." },
            { 5, "Puppet master calculating new state." },
        };

        private static readonly Dictionary<int, string> _debugMessages = new()
        {
            { 0, "(╯°□°）╯︵ ʎʇᴉun" },
            { 1, "Foobar?" },
            { 2, "/* _player.DevMode = false // uncomment this line */" },
            { 3, "bug/CGGO-734 test" },
            { 4, "To malloc or not to malloc?" },
            { 5, "Code is F# up beyond all recognition" },
        };

        IEnumerator Loop()
        {
            for (;;)
            {
                var waitTime = Random.Range(timeSpan.x, timeSpan.y);
                var mode = Random.Range(0, 3);
                var messageRange = Random.Range(0, 6);

                switch (mode)
                {
                    case 0:
                        GameLogger.LogError($"Critical failure. Error code : {ErrorCodeGenerator()}");
                        break;
                    case 1:
                        GameLogger.LogDebug(_debugMessages[messageRange]);
                        break;
                    default:
                        GameLogger.LogInfo(_logMessages[messageRange]);
                        break;
                }

                yield return new WaitForSecondsRealtime(waitTime);
            }
        }

        private void OnEnable()
        {
            StartCoroutine(Loop());
        }

        private void OnDestroy()
        {
            StopCoroutine(Loop());
        }
    }
}