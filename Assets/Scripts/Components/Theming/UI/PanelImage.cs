using UnityEngine.UI;

namespace Components.Theming.UI
{
    public class PanelImage : Image, IThemeable
    {
        protected ITheme _theme;

        public ITheme Theme => _theme;

        public void SetTheme(ITheme theme)
        {
            _theme = theme;

            if (theme != null)
            {
                this.color = theme.Panel;
            }
        }
    }
}