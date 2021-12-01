using UnityEngine;

namespace Utilities
{
    public static class ColorStringUtility
    {
        public static string ToColored(this string text, Color32 color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{text}</color>";
        }
    }
}