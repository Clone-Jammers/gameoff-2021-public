using TMPro;

namespace Components.Theming.UI
{
    public class TitleText : TextMeshProUGUI, IThemeable
    {
        protected ITheme _theme;

        public ITheme Theme => _theme;

        public void SetTheme(ITheme theme)
        {
            _theme = theme;

            if (theme != null)
            {
                this.color = theme.SecondaryColor;
                this.font = theme.TitleFont;
                this.fontSize = theme.TitleSize;
            }
        }
    }
}