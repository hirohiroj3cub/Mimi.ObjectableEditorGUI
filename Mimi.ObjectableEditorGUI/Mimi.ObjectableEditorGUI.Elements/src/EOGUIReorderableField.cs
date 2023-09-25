using System;
using UnityEditorInternal;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIReorderableField<T> : EOGUIReorderableField<EOGUIReorderableField<T>, EOGUIReorderableFieldElement<T>, T>
    {
        protected override EOGUIReorderableFieldElement<T> CreateNewElement()
        {
            return new EOGUIReorderableFieldElement<T>(ReorderableList);
        }
    }

    public abstract class EOGUIReorderableField<TSelf, TElement, T> : EOGUIReorderable<TSelf, TElement>
        where TSelf : EOGUIReorderableField<TSelf, TElement, T>
        where TElement : class, IEOGUIElementField<T>
    {
        private static readonly Func<TElement, T> getter = e => e.Value;
        private static readonly Action<TElement, T> setter = (e, value) => e.Value = value;

        protected EOGUIReorderableField() : base()
        {
            Values = new EOGUIElementChildrenValueList<TSelf, TElement, T>(ThisT, CreateNewElement, getter, setter);
            ReorderableList.onReorderCallbackWithDetails += (list, oldIndex, newIndex) =>
            {
                (Values[oldIndex], Values[newIndex]) = (Values[newIndex], Values[oldIndex]);
            };
        }

        public EOGUIElementChildrenValueList<TSelf, TElement, T> Values { get; }

        protected override TElement CreateNewElement(TElement baseElement)
        {
            var e = CreateNewElement();
            e.Value = baseElement.Value;
            return e;
        }
    }
}