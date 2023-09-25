using System;
using System.Runtime.InteropServices;

namespace Mimi.ObjectableEditorGUI.GUIUtility
{
    [StructLayout(LayoutKind.Auto)]
    public ref struct GUIFontSettingScope
    {
        private GUIFontSetting fontSetting;

        public GUIFontSettingScope(GUIFontSetting fontSetting)
        {
            this.fontSetting = fontSetting ?? throw new ArgumentNullException(nameof(fontSetting));
        }

        public void Dispose()
        {
            if (fontSetting != null)
            {
                fontSetting.Restore();
                fontSetting = null;
            }
        }
    }
}