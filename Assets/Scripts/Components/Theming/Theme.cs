using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Components.Theming
{
    [CreateAssetMenu(menuName = "Theme/Create Theme", fileName = "Custom Theme")]
    [InlineEditor()]
    public class Theme : ScriptableObject, ITheme
    {
        [FoldoutGroup("Auxiliary Colors")] [SerializeField] [LabelWidth(150)]
        private Color _error;

        [FoldoutGroup("Auxiliary Colors")] [SerializeField] [LabelWidth(150)]
        private Color _danger;

        [FoldoutGroup("Auxiliary Colors")] [SerializeField] [LabelWidth(150)]
        private Color _warning;

        [FoldoutGroup("Auxiliary Colors")] [SerializeField] [LabelWidth(150)]
        private Color _success;

        [FoldoutGroup("Auxiliary Colors")] [SerializeField] [LabelWidth(150)]
        private Color _uncommon;

        [FoldoutGroup("Surface Colors")] [SerializeField] [LabelWidth(150)]
        private Color _background;

        [FoldoutGroup("Surface Colors")] [SerializeField] [LabelWidth(150)]
        private Color _panel;

        [FoldoutGroup("Surface Colors")] [SerializeField] [LabelWidth(150)]
        private Color _active;

        [FoldoutGroup("Surface Colors")] [SerializeField] [LabelWidth(150)]
        private Color _selected;

        [FoldoutGroup("Text Colors")] [SerializeField] [LabelWidth(150)]
        private Color _disabled;

        [FoldoutGroup("Text Colors")] [SerializeField] [LabelWidth(150)]
        private Color _secondaryText;

        [FoldoutGroup("Text Colors")] [SerializeField] [LabelWidth(150)]
        private Color _primaryText;

        [FoldoutGroup("Accent Colors")] [SerializeField] [LabelWidth(150)]
        private Color _highlight;

        [FoldoutGroup("Accent Colors")] [SerializeField] [LabelWidth(150)]
        private Color _primaryAccent;

        [FoldoutGroup("Accent Colors")] [SerializeField] [LabelWidth(150)]
        private Color _secondaryAccent;

        [FoldoutGroup("Accent Colors")] [SerializeField] [LabelWidth(150)]
        private Color _tertiaryAccent;

        [FoldoutGroup("Fonts")] [SerializeField] [LabelWidth(150)]
        private TMP_FontAsset _titleFont;

        [FoldoutGroup("Fonts")] [SerializeField] [LabelWidth(150)]
        private TMP_FontAsset _textFont;

        [FoldoutGroup("Fonts")] [SerializeField] [LabelWidth(150)]
        private TMP_FontAsset _subTextFont;

        [FoldoutGroup("Font Sizes")] [SerializeField] [LabelWidth(150)] [MinValue(1)]
        private float _titleSize;

        [FoldoutGroup("Font Sizes")] [SerializeField] [LabelWidth(150)] [MinValue(1)]
        private float _textSize;

        [FoldoutGroup("Font Sizes")] [SerializeField] [LabelWidth(150)] [MinValue(1)]
        private float _subTextSize;

        public Color Error => _error;

        public Color Danger => _danger;

        public Color Warning => _warning;

        public Color Success => _success;

        public Color Uncommon => _uncommon;

        public Color Background => _background;

        public Color Panel => _panel;

        public Color Active => _active;

        public Color Selected => _selected;

        public Color Disabled => _disabled;

        public Color SecondaryColor => _secondaryText;

        public Color PrimaryColor => _primaryText;

        public Color Highlight => _highlight;

        public Color PrimaryAccent => _primaryAccent;

        public Color SecondaryAccent => _secondaryAccent;

        public Color TertiaryAccent => _tertiaryAccent;

        public TMP_FontAsset TitleFont => _titleFont;

        public TMP_FontAsset TextFont => _textFont;

        public TMP_FontAsset SubTextFont => _subTextFont;

        public float TitleSize => _titleSize;

        public float TextSize => _textSize;

        public float SubTextSize => _subTextSize;
    }
}