using Components.Theming;
using Sirenix.OdinInspector;
using System.Text;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Components.Terminal
{
    public class TMPTerminal : MonoBehaviour, IThemeable
    {
        [SerializeField] [OnValueChanged("InternalSetTitle")]
        private string _title;

        [SerializeField] private TMP_Text _textElement;
        [SerializeField] private TMP_Text _titleElement;
        [SerializeField] private int _lineCount;
        [SerializeField] private ScrollRect _scrollRect;

        public ITheme Theme { get; private set; }

        public void Write(string text)
        {
            WriteToBuffer(text);
        }

        public void WriteLine(string text)
        {
            WriteToBuffer(text + '\n');
        }

        public void OverwriteLine(string text)
        {
            WriteToBuffer('\r' + text);
        }

        public void SetTitle(string value)
        {
            InternalSetTitle(value);
        }

        private void WriteToBuffer(string text)
        {
            var buffer = GetLastLines(new[]
            {
                _textElement.text,
                text
            });

            _textElement.SetText(buffer);
        }

        public void SetTheme(ITheme theme)
        {
            Theme = theme;
        }

        private void OnGUI()
        {
            // Force scroll rect position to bottom onGUI render otherwise it jumps back for one line?
            _scrollRect.verticalNormalizedPosition = 0f;
        }

        public void SetActive(bool value) => this.gameObject.SetActive(value);

        private void Awake()
        {
            InternalSetTitle(_title);
        }

        private char[] GetLastLines(string[] texts)
        {
            var builder = new StringBuilder();

            var allowWrite = true;
            var lineCount = -1;

            for (int tI = texts.Length - 1; tI >= 0; tI--)
            {
                var text = texts[tI];

                for (int i = text.Length - 1; i >= 0 && lineCount <= _lineCount; i--)
                {
                    var ch = text[i];

                    if (ch == '\r')
                    {
                        allowWrite = false;
                    }
                    else if (ch == '\n')
                    {
                        lineCount++;

                        if (_lineCount == lineCount)
                            break;

                        allowWrite = true;
                        builder.Append(ch);
                    }
                    else if (allowWrite)
                    {
                        builder.Append(ch);
                    }
                }
            }

            var buffer = builder.ToString().ToCharArray();
            Array.Reverse(buffer);

            return buffer;
        }

        private void InternalSetTitle(string value)
        {
            if (_titleElement != null)
            {
                _titleElement.text = value;
            }
        }
    }
}