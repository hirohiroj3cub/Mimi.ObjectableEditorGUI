using System;
using System.Collections.Generic;
using UnityEditor;

namespace Mimi.ObjectableEditorGUI.Elements
{

    public sealed class EOGUIField<T> : EOGUIField<EOGUIField<T>, T>
    {
        public EOGUIField(T defaultValue) : base(defaultValue)
        {
        }
    }

    public abstract class EOGUIField<TSelf, T> : EOGUIElement<TSelf>, IEOGUIElementField<TSelf, T>
        where TSelf : EOGUIField<TSelf, T>
    {
        private T value;

        public event Action<T>? OnChangedValue;

        public EOGUIField(T defaultValue)
        {
            value = defaultValue!;

            if (EOGUIFieldDefaultGUIs<int>.CheckInit<T>())
            {
                EOGUIFieldDefaultGUIs<int>.Init(EditorGUI.IntField, EditorGUI.DelayedIntField);
            }
            else if (EOGUIFieldDefaultGUIs<float>.CheckInit<T>())
            {
                EOGUIFieldDefaultGUIs<float>.Init(EditorGUI.FloatField, EditorGUI.DelayedFloatField);
            }
            else if (EOGUIFieldDefaultGUIs<double>.CheckInit<T>())
            {
                EOGUIFieldDefaultGUIs<double>.Init(EditorGUI.DelayedDoubleField, EditorGUI.DelayedDoubleField);
            }
            else if (EOGUIFieldDefaultGUIs<string>.CheckInit<T>())
            {
                EOGUIFieldDefaultGUIs<string>.Init(EditorGUI.TextField, EditorGUI.DelayedTextField);
            }

            if (EOGUIFieldDefaultGUIs<T>.GUIField == null)
            {
                throw new ArgumentException(nameof(T));
            }
        }

        public EqualityComparer<T> Comparer { get; set; } = EqualityComparer<T>.Default;

        public override EOGUIHeightType HeightType => EOGUIHeightType.Static;

        public T Value
        {
            get => value;
            set
            {
                if (!Comparer.Equals(this.value, value))
                {
                    this.value = value;
                    OnChangedValue?.Invoke(value);
                }
            }
        }
        public bool IsDelayed { get; set; }

        public override float GetElementHeight()
        {
            return EOGUIFieldDefaultGUIs<T>.Height(ValidLabel, value);
        }

        public override void OnElementGUI()
        {
            Value = EOGUIFieldDefaultGUIs<T>.GUIField!.Invoke(Status.rect, ValidLabel, value, IsDelayed);
            EOGUIFieldClipBoard.ContextMenu(this);
        }
    }
}