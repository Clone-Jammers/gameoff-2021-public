using Components.Logger;
using System;
using System.Linq;
using Components.Terminal;
using Components.Theming;
using UnityEngine;
using Utilities;

namespace Components.Sinks
{
    [Serializable]
    public class TerminalSink : Sink, IThemeable
    {
        [SerializeField] private TMPTerminal _terminal;
        protected ITheme _theme;

        protected override void onEvent(string message, LogTypes types)
        {
            if (message?.Length < 1) return;

            var outputMessage = message;
            var trimmed = message.AsSpan().TrimStart(' ');
            var hasReturn = false;

            // check return cariage before inserting color codes
            if (trimmed.Length > 1 && trimmed[0] == '\r')
            {
                outputMessage = trimmed.Slice(1).ToString();
                hasReturn = true;
            }

            var text = types switch
            {
                LogTypes.Debug => ("[DEBUG] - " + outputMessage).ToColored(_theme.Warning),
                LogTypes.Error => ("[ERROR] - " + outputMessage).ToColored(_theme.Error),
                LogTypes.Info => ("[INFO]  - " + outputMessage).ToColored(_theme.Uncommon),
                LogTypes.Warning => ("[WARN]  - " + outputMessage).ToColored(_theme.Danger),
                _ => outputMessage
            };

            _terminal.WriteLine(hasReturn ? '\r' + text : text);
        }

        public ITheme Theme => _theme;

        public void SetTheme(ITheme theme)
        {
            if (theme != null)
            {
                _theme = theme;
            }
        }
    }
}