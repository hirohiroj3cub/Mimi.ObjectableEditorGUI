using UnityEditor;

namespace Mimi.ObjectableEditorGUI.GUIUtility
{
    public static class BackingFieldAccesser
    {
        public static SerializedProperty FindBackingField(this SerializedObject serializedObject, string propertyName)
        {
            return serializedObject.FindProperty(GetBackingFieldName(propertyName));
        }

        public static SerializedProperty FindBackingFieldRelative(this SerializedProperty serializedProperty, string relativePropertyName)
        {

            return serializedProperty.FindPropertyRelative(GetBackingFieldName(relativePropertyName));
        }

        public static string GetBackingFieldName(this string propertyName)
        {
            return $"<{propertyName}>k__BackingField";
        }
    }
}