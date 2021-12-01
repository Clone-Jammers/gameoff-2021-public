using TMPro;
using UnityEngine;

namespace Components.Theming
{
    public interface ITheme
    {
        public Color Error { get; }
        public Color Danger { get; }
        public Color Warning { get; }
        public Color Success { get; }
        public Color Uncommon { get; }
        
        public Color Background { get; }
        public Color Panel { get; }
        public Color Active { get; }
        public Color Selected { get; }
        
        public Color Disabled { get; }
        public Color SecondaryColor { get; }
        public Color PrimaryColor { get; }
        
        public Color Highlight { get; }
        public Color PrimaryAccent { get; }
        public Color SecondaryAccent { get; }
        public Color TertiaryAccent { get; }

        public TMP_FontAsset TitleFont { get; }
        public TMP_FontAsset TextFont { get; }
        public TMP_FontAsset SubTextFont { get; }

        public float TitleSize { get; }
        public float TextSize { get; }
        public float SubTextSize { get; }
    }
}