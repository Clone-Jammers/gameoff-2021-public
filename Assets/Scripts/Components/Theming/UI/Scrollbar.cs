using UnityEditor;
using UnityEngine.UI;

namespace Components.Theming.UI
{
    public class Scrollbar : UnityEngine.UI.Scrollbar, IThemeable
    {
        private ITheme _theme;

        public ITheme Theme => _theme;

        public void SetTheme(ITheme theme)
        {
            _theme = theme;

            if (theme != null)
            {
                var originalBlock = this.colors;
                var block = new ColorBlock()
                {
                    colorMultiplier = originalBlock.colorMultiplier,
                    disabledColor = theme.Disabled,
                    fadeDuration = originalBlock.fadeDuration,
                    highlightedColor = theme.Highlight,
                    normalColor = theme.SecondaryColor,
                    pressedColor = theme.PrimaryAccent,
                    selectedColor = theme.Active
                };

                this.colors = block;
            }
        }
    }
}