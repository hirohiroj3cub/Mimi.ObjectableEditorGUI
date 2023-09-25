using System.Runtime.InteropServices;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.GUIUtility
{
    [StructLayout(LayoutKind.Auto)]
    public ref struct GUIColorScope
    {
        private bool isActiveContent;
        private bool isActiveBackground;
        public Color OldContentColor { get; }
        public Color OldBackgroundColor { get; }

        public GUIColorScope(Color? content, Color? background)
        {
            if (isActiveContent = content != null)
            {
                OldContentColor = GUI.contentColor;
                GUI.contentColor = content.Value;
            }
            else
            {
                OldContentColor = default;
            }

            if (isActiveBackground = background != null)
            {
                OldBackgroundColor = GUI.backgroundColor;
                GUI.backgroundColor = background.Value;
            }
            else
            {
                OldBackgroundColor = default;
            }
        }

        public void Dispose()
        {
            if (isActiveContent)
            {
                GUI.contentColor = OldContentColor;
                isActiveContent = false;
            }
            if (isActiveBackground)
            {
                GUI.backgroundColor = OldBackgroundColor;
                isActiveBackground = false;
            }
        }
    }
}