using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.GUIUtility
{
    public class GUIFontSetting
    {
        public GUIFontSetting(GUIStyle style = null, FontStyle? fontStyle = null, int? fontSize = null, Font font = null)
        {
            Style = style ?? EditorStyles.label;
            FontStyle = fontStyle;
            FontSize = fontSize;
            Font = font;
            OldFont = style.font;
            OldFontStyle = style.fontStyle;
            OldFontSize = style.fontSize;
        }

        public GUIStyle Style { get; set; }
        public FontStyle? FontStyle { get; set; }
        public int? FontSize { get; set; }
        public Font Font { get; set; }
        public FontStyle OldFontStyle { get; }
        public int OldFontSize { get; }
        public Font OldFont { get; }

        public void Set()
        {
            if (FontStyle != null) Style.fontStyle = FontStyle.Value;
            if (FontSize != null) Style.fontSize = FontSize.Value;
            if (Font != null) Style.font = Font;
        }

        public void Restore()
        {
            Style.fontStyle = OldFontStyle;
            Style.fontSize = OldFontSize;
            Style.font = Font;
        }
    }
}