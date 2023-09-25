using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public static class EOGUIFieldDefaultGUIs<T>
    {
        public delegate T GUIFuncWithDelayed(Rect rect, GUIContent label, T value, bool isDelayed);
        public delegate T GUIFunc(Rect rect, GUIContent label, T value);
        public delegate float GUIHeight(GUIContent label, T value);

        public static GUIFuncWithDelayed GUIField { get; set; }
        public static GUIHeight Height { get; set; } = (GUIContent label, T value) => EditorGUIUtility.singleLineHeight;

        public static bool CheckInit<T2>()
        {
            return typeof(T2) == typeof(T) && GUIField == null;
        }

        public static void Init(GUIFunc func, GUIFunc funcDelayed)
        {
            GUIField = (Rect rect, GUIContent label, T value, bool isDelayed) =>
            {
                if (isDelayed && funcDelayed != null)
                {
                    return funcDelayed.Invoke(rect, label, value);
                }
                else
                {
                    return func.Invoke(rect, label, value);
                }
            };
        }
    }
}