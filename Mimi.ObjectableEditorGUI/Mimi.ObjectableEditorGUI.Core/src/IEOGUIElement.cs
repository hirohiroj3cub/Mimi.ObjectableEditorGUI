using Mimi.ObjectableEditorGUI.Context;
using System;
using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI
{
    public interface IEOGUIElement
    {
        public event Action<IEOGUIElement> OnBeginElement;
        public event Action<IEOGUIElement> OnEndElement;

        bool IsDirty { get; }
        EOGUIContext Context { get; }
        EOGUIContextStatus Status { get; }
        string Name { get; }
        EOGUIElementOptions Options { get; }
        EOGUIHeightType HeightType { get; }
        IEOGUIElementParent Parent { get; }
        int Index { get; }
        EOGUIElementExtensionsResource ExtensionsResource { get; }
        GUIContent Label { get; set; }
        SerializedProperty SerializedProperty { get; }
        GUIContent ValidLabel { get; }

        void BeginElement();
        void OnElementContextUpdate();
        void UpdateSerializedProperty();
        void OnElementPreUpdate();
        float GetElementHeight();
        void OnElementGUI();
        void EndElement();
    }

    public interface IEOGUIElement<TSelf> : IEOGUIElementChild
        where TSelf : class, IEOGUIElement, IEOGUIElement<TSelf>
    {
        public new EOGUIElementOptions<TSelf> Options { get; }
    }
}