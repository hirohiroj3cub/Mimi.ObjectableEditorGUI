﻿using Mimi.ObjectableEditorGUI.Context;
using System;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIReorderablePropertyField<T> : EOGUIReorderablePropertyField<EOGUIReorderablePropertyField<T>, EOGUIReorderablePropertyFieldElement<T>, T>
    {
        public EOGUIReorderablePropertyField(EOGUIContextWriterSerializedProperty serializedPropertyWriter) : base(serializedPropertyWriter)
        {
        }

        protected override EOGUIReorderablePropertyFieldElement<T> CreateNewElement()
        {
            return new EOGUIReorderablePropertyFieldElement<T>();
        }
    }

    public abstract class EOGUIReorderablePropertyField<TSelf, TElement, T> : EOGUIReorderableProperty<TSelf, TElement>
        where TSelf : EOGUIReorderablePropertyField<TSelf, TElement, T>
        where TElement : class, IEOGUIElementField<T>
    {
        private static readonly Func<TElement, T> getter = e => e.Value;
        private static readonly Action<TElement, T> setter = (e, value) => e.Value = value;

        public EOGUIElementChildrenValueList<TSelf, TElement, T> Values { get; }

        protected EOGUIReorderablePropertyField(EOGUIContextWriterSerializedProperty serializedPropertyWriter) : base(serializedPropertyWriter)
        {
            Values = new EOGUIElementChildrenValueList<TSelf, TElement, T>(ThisT, CreateNewElement, getter, setter);
        }

        protected override TElement CreateNewElement(TElement baseElement)
        {
            var e = CreateNewElement();
            e.Value = baseElement.Value;
            return e;
        }
    }
}