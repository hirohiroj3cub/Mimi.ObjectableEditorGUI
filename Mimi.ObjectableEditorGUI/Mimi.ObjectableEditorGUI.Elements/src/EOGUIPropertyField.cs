using Mimi.ObjectableEditorGUI.Context;
using System;
using System.Data.SqlTypes;
using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIPropertyField<T> : EOGUIPropertyField<EOGUIPropertyField<T>, T>
    {
        public EOGUIPropertyField() : base(new EOGUIContextWriterSerializedProperty())
        {
        }

        public EOGUIPropertyField(EOGUIContextWriterSerializedProperty serializedPropertySelector) : base(serializedPropertySelector)
        {
        }
    }

    public abstract class EOGUIPropertyField<TSelf, T> : EOGUIProperty<TSelf>, IEOGUIElementField<TSelf, T>
        where TSelf : EOGUIPropertyField<TSelf, T>
    {
        private struct InitValueObject
        {
            private T value;
            private bool hasValue;
            private bool isUsed;

            public T Value
            {
                readonly get => value;
                set
                {
                    this.value = value;
                    hasValue = true;
                }
            }

            public readonly bool HasValue => hasValue;

            public bool IsUsed
            {
                readonly get => isUsed;
                set
                {
                    isUsed = value;
                }
            }
        }

        private bool throwedGetterError = false;
        private bool throwedSetterError = false;
        private InitValueObject initValue;

        public event Action<T>? OnChangedValue;

        protected EOGUIPropertyField(EOGUIContextWriterSerializedProperty serializedPropertySelector) : base(serializedPropertySelector)
        {
            Getter = EOGUIPropertyFieldDefaultAccesser<T>.Getter;
            Setter = EOGUIPropertyFieldDefaultAccesser<T>.Setter;
        }

        public Func<SerializedProperty, T>? Getter { get; set; }
        public Action<SerializedProperty, T>? Setter { get; set; }

        public override void OnElementContextUpdate()
        {
            base.OnElementContextUpdate();
            if (SerializedProperty != null && initValue.HasValue && !initValue.IsUsed)
            {
                Setter!.Invoke(SerializedProperty, initValue.Value);
                initValue.IsUsed = true;
            }
        }

        public bool HasValue => initValue.HasValue || SerializedProperty != null;

        public T Value
        {
            get
            {
                try
                {
                    if (SerializedProperty == null)
                    {
                        return initValue.Value;
                    }
                    else
                    {
                        return Getter!.Invoke(SerializedProperty);
                    }
                }
                catch (Exception e)
                {
                    if (!throwedGetterError)
                    {
                        Debug.LogException(e);
                        throwedGetterError = true;
                    }
                    return initValue.Value;
                }
            }

            set
            {
                try
                {
                    if (SerializedProperty == null)
                    {
                        initValue.Value = value;
                    }
                    else
                    {
                        Setter!.Invoke(SerializedProperty, value);
                        OnChangedValue?.Invoke(value);
                    }
                }
                catch (Exception e)
                {
                    if (!throwedSetterError)
                    {
                        Debug.LogException(e);
                        throwedSetterError = true;
                    }
                }
            }
        }

        public override void OnElementGUI()
        {
            EditorGUI.BeginChangeCheck();
            base.OnElementGUI();
            if (EditorGUI.EndChangeCheck())
            {
                OnChangedValue?.Invoke(Value);
            }
        }
    }
}