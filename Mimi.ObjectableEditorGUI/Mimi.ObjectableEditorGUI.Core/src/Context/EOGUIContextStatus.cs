using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Context
{
    public class EOGUIContextStatus
    {
        public Rect rect;
        public GUIContent label;
        private SerializedProperty serializedProperty;
        public GUIContent serializedPropertyLabel;
        private Dictionary<object, object> customContext;

        public void Init(SerializedProperty serializedProperty, in Rect rect, Dictionary<object, object> customContext)
        {
            serializedPropertyLabel = new GUIContent(serializedProperty.displayName);
            SerializedProperty = serializedProperty ?? throw new ArgumentNullException(nameof(serializedProperty));
            this.rect = rect;
            label = null;
            this.customContext = customContext;
            customContext.Clear();
        }

        public void Set(EOGUIContextStatusCache cache, IEOGUIElement element)
        {
            serializedPropertyLabel = cache.SerializedPropertyLabel;
            SerializedProperty = cache.SerializedProperty;
            if (element != null)
            {
                rect = new Rect(0, 0, element.ExtensionsResource.cachedWidth, element.ExtensionsResource.cachedHeight.current);

            }
            label = cache.Label;
            customContext = (Dictionary<object, object>)cache.CustomContext;
        }

        public SerializedProperty SerializedProperty
        {
            get => serializedProperty;
            set
            {
                serializedProperty = value;
                serializedPropertyLabel.text = serializedProperty.displayName;
            }
        }
        public GUIContent ValidLabel => label ?? serializedPropertyLabel;
        public Dictionary<object, object> CustomContext => customContext;
    }
}