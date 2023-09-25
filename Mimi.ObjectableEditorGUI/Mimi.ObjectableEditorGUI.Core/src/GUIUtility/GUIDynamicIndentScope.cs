using System.Runtime.InteropServices;
using UnityEditor;

namespace Mimi.ObjectableEditorGUI.GUIUtility
{
    [StructLayout(LayoutKind.Auto)]
    public ref struct GUIDynamicIndentScope
    {
        private bool isActive;
        private readonly int indent;

        public GUIDynamicIndentScope(int indent)
        {
            isActive = true;
            this.indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = indent;
        }

        public void Dispose()
        {
            if (isActive)
            {
                EditorGUI.indentLevel = indent;
                isActive = false;
            }
        }
    }
}