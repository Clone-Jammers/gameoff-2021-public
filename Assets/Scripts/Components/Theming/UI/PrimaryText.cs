using TMPro;

namespace Components.Theming.UI
{
    public class PrimaryText : TextMeshProUGUI, IThemeable
    {
        protected ITheme _theme;

        public ITheme Theme => _theme;

        public void SetTheme(ITheme theme)
        {
            _theme = theme;
            
            if (theme != null)
            {
                this.color = theme.PrimaryColor;
                this.font = theme.TextFont;
                this.fontSize = theme.TextSize;
            }
        }
    }
}