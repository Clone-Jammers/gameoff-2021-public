using Sirenix.OdinInspector;
using UnityEngine;

namespace Components.Theming
{
    public class ThemeProvider : MonoBehaviour
    {
        [SerializeField] [OnValueChanged("LoadTheme", true)]
        private Theme _theme;

        public void Awake()
        {
            LoadTheme();
        }

        private void LoadTheme()
        {
            var themeableUIs = GetComponentsInChildren<IThemeable>(true);

            if (themeableUIs == null || themeableUIs.Length <= 0) return;
            
            foreach (var ui in themeableUIs)
            {
                ui.SetTheme(_theme);
            }
        }
    }
}