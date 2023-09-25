using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public static class EOGUIReorderableUtility
    {
        public static void LabelTextVaild<T>(GUIContent label, int index, T value)
        {
            var str = value?.ToString();
            if (typeof(T) == typeof(string))
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    str = $"Element {index}";
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(str) || str.StartsWith(typeof(T).FullName, System.StringComparison.Ordinal))
                {
                    str = $"Element {index}";
                }
            }

            label.text = str;
            var width = EditorStyles.label.CalcSize(label).x;
            while (width > 120)
            {
                str = str.TrimEnd('.')[..^1] + "..";
                label.text = str;
                width = EditorStyles.label.CalcSize(label).x;
            }
        }
    }
}