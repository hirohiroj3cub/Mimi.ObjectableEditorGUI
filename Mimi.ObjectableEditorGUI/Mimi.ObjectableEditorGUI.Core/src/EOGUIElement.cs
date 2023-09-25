using Mimi.ObjectableEditorGUI.Context;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI
{
    public abstract class EOGUIElement<TSelf> : EOGUIElement, IEOGUIElement<TSelf> where TSelf : EOGUIElement<TSelf>
    {
        public TSelf ThisT { get; }

        public EOGUIElement()
        {
            ThisT = (TSelf)this;
            Options = new EOGUIElementOptions<TSelf>()
            {
                Margin = new RectOffset(1, 1, 0, 2),
            };
        }

        public new EOGUIElementOptions<TSelf> Options { get; set; }

        public sealed override EOGUIElementOptions GetOptions()
        {
            return Options;
        }
    }

    public abstract class EOGUIElement : IEOGUIElement, IEOGUIElementChild
    {
        private readonly EOGUIElementExtensionsResource extensionsResource = new EOGUIElementExtensionsResource();

        public EOGUIElement()
        {
            Name = GetType().Name;
        }

        private protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<IEOGUIElement> OnBeginElement;
        public event Action<IEOGUIElement> OnEndElement;

        public bool IsDirty { get; private set; }
        public EOGUIContext Context => extensionsResource.context;
        public EOGUIContextStatus Status => extensionsResource.context.Status;
        public string Name { get; set; }
        public EOGUIElementOptions Options => GetOptions();
        public abstract EOGUIHeightType HeightType { get; }
        public IEOGUIElementParent Parent { get; private set; }
        public int Index => Parent?.IndexOf(this) ?? -1;
        EOGUIElementExtensionsResource IEOGUIElement.ExtensionsResource => extensionsResource;
        public GUIContent Label { get; set; }
        public SerializedProperty SerializedProperty { get; private set; }
        public GUIContent ValidLabel => Label ?? Status?.ValidLabel;

        public abstract EOGUIElementOptions GetOptions();

        public virtual void BeginElement()
        {
            OnBeginElement?.Invoke(this);
            IsDirty = true;
        }

        public virtual void OnElementContextUpdate()
        {
            UpdateSerializedProperty();
            if (Label != null)
            {
                Status.label = Label;
            }
        }

        public void UpdateSerializedProperty()
        {
            SerializedProperty = Status.SerializedProperty;
        }

        public virtual void OnElementPreUpdate() { }

        public abstract void OnElementGUI();

        public abstract float GetElementHeight();

        public virtual void EndElement()
        {
            IsDirty = false;
            OnEndElement?.Invoke(this);
        }

        public override string ToString() => Name;

        void IEOGUIElementChild.SetParent(IEOGUIElementParent parent)
        {
            Parent = parent;
        }
    }
}