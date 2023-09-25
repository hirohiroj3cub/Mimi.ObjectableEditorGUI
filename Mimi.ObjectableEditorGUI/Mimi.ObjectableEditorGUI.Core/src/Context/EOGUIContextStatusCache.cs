using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Context
{
    public class EOGUIContextStatusCache
    {
        public bool IsInitialized { get; private set; }
        public SerializedProperty SerializedProperty { get; private set; }
        //public Rect Rect { get; private set; }
        public GUIContent Label { get; private set; }
        public GUIContent SerializedPropertyLabel { get; private set; }
        public IReadOnlyDictionary<object, object> CustomContext { get; private set; }

        public void Set(EOGUIContextStatus status)
        {
            IsInitialized = true;
            SerializedPropertyLabel = status.serializedPropertyLabel;
            SerializedProperty = status.SerializedProperty;
            //Rect = status.rect;
            Label = status.label;
            CustomContext = status.CustomContext;
        }
    }
}