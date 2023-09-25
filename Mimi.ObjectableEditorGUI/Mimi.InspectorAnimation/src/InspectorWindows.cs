using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Mimi.InspectorAnimation
{
    public static class InspectorWindows
    {
        private static Type? editorType = null;
        public static Type EditorType => editorType ??= ActiveEditorTracker.sharedTracker.activeEditors[0].GetType();

        public static Editor[] GetEditors()
        {
            var editors = Resources.FindObjectsOfTypeAll(EditorType)
                          .Cast<Editor>()
                          .ToArray();

            return editors;
        }

        public static Editor[] GetEditors(object? target)
        {
            return target switch
            {
                Object obj => GetEditors(obj),
                Object[] objects => GetEditors(objects),
                SerializedObject so => GetEditors(so),
                _ => throw new ArgumentException(nameof(target))
            };
        }

        public static Editor[] GetEditors(SerializedObject serializedObject)
        {
            var editors = Resources.FindObjectsOfTypeAll(EditorType)
                          .Cast<Editor>()
                          .Where(editor => serializedObject == editor.serializedObject)
                          .ToArray();

            return editors;
        }

        public static Editor[] GetEditors(Object[] objects)
        {
            var editors = Resources.FindObjectsOfTypeAll(EditorType)
                          .Cast<Editor>()
                          .Where(editor => objects.Intersect(editor.targets).Count() > 0)
                          .ToArray();

            return editors;
        }

        public static Editor[] GetEditors(Object obj)
        {
            var editors = Resources.FindObjectsOfTypeAll(EditorType)
                          .Cast<Editor>()
                          .Where(editor => obj == editor.target)
                          .ToArray();

            return editors;
        }
    }
}