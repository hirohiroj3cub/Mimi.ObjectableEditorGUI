using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Context
{
    public class EOGUIContextStatusStack
    {
        private Element[] elements = new Element[8];
        public int Count { get; private set; } = 0;

        [StructLayout(LayoutKind.Auto)]
        private struct Element
        {
            public Dictionary<object, object> customContext;
            public GUIContent label;
            public Rect rect;
            public SerializedProperty serializedProperty;

            public void Clear()
            {
                customContext ??= new Dictionary<object, object>();
                customContext.Clear();
                label = null;
                serializedProperty = null;
            }

            public void SetFrom(EOGUIContextStatus source)
            {
                customContext ??= new Dictionary<object, object>();
                customContext.Clear();
                customContext.AddRange(source.CustomContext);
                label = source.label;
                rect = source.rect;
                serializedProperty = source.SerializedProperty;
            }

            public void SetTo(EOGUIContextStatus source)
            {
                customContext ??= new Dictionary<object, object>();
                source.CustomContext.Clear();
                source.CustomContext.AddRange(customContext);
                source.label = label;
                source.rect = rect;
                source.SerializedProperty = serializedProperty;
            }
        }

        public void Init()
        {
            Count = 0;
        }

        public void Push(EOGUIContextStatus source)
        {
            if (elements.Length <= Count)
            {
                Debug.Log(Count);
                var newArray = new Element[elements.Length * 2];
                Array.Copy(elements, newArray, elements.Length);
                elements = newArray;
            }

            elements[Count++].SetFrom(source);
        }

        public void Pop(EOGUIContextStatus source)
        {
            ref var peak = ref elements[--Count];
            peak.SetTo(source);
            peak.Clear();
        }
    }
}