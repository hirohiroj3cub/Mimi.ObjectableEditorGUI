using Mimi.ObjectableEditorGUI.Context;
using System;
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
        private bool throwedGetterError = false;
        private bool throwedSetterError = false;
        private bool hasInitValue;
        private T initValue;

        public event Action<T> OnChangedValue;

        protected EOGUIPropertyField(EOGUIContextWriterSerializedProperty serializedPropertySelector) : base(serializedPropertySelector)
        {
            Getter = EOGUIPropertyFieldDefaultAccesser<T>.Getter;
            Setter = EOGUIPropertyFieldDefaultAccesser<T>.Setter;
        }

        public Func<SerializedProperty, T> Getter { get; set; }
        public Action<SerializedProperty, T> Setter { get; set; }

        public override void OnElementContextUpdate()
        {
            base.OnElementContextUpdate();
            if (hasInitValue && SerializedProperty != null)
            {
                Setter.Invoke(SerializedProperty, initValue);
                hasInitValue = false;
            }
        }

        public T Value
        {
            get
            {
                try
                {
                    if (SerializedProperty == null)
                    {
                        return initValue;
                    }
                    else
                    {
                        return Getter.Invoke(SerializedProperty);
                    }
                }
                catch (Exception e)
                {
                    if (!throwedGetterError)
                    {
                        Debug.LogException(e);
                        throwedGetterError = true;
                    }
                    return default;
                }
            }

            set
            {
                try
                {
                    if (SerializedProperty == null)
                    {
                        hasInitValue = true;
                        initValue = value;
                    }
                    else
                    {
                        Setter.Invoke(SerializedProperty, value);
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