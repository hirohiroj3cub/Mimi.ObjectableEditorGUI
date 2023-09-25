using System.Runtime.InteropServices;
using UnityEditor;

namespace Mimi.ObjectableEditorGUI.GUIUtility
{
    [StructLayout(LayoutKind.Auto)]
    public ref struct GUILabelWidthScope
    {
        private bool isActive;
        private readonly float width;

        public GUILabelWidthScope(float width)
        {
            isActive = true;
            this.width = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = width;
        }

        public void Dispose()
        {
            if (isActive)
            {
                EditorGUIUtility.labelWidth = width;
                isActive = false;
            }
        }
    }
}