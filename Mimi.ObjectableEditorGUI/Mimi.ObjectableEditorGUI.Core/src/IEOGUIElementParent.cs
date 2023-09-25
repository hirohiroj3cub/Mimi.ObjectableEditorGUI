using System;
using System.Collections;
using System.Collections.Generic;

namespace Mimi.ObjectableEditorGUI
{
    public interface IEOGUIElementParent<TSelf, TElement> : IEOGUIElement<TSelf>, IEOGUIElementParent<TElement>
        where TElement : IEOGUIElementChild
        where TSelf : class, IEOGUIElementParent<TSelf, TElement>
    {
    }

    public interface IEOGUIElementParent<TElement> : IEOGUIElementParent, IList<TElement>
        where TElement : IEOGUIElement, IEOGUIElementChild
    {
        public IList<TElement> AsList => this;

        void DelayAdd(Func<IEOGUIElementParent<TElement>, TElement> element);
        void DelayInsert(Func<IEOGUIElementParent<TElement>, (int, TElement)> element);
        void DelayRemove(Func<IEOGUIElementParent<TElement>, TElement, bool> selector);
        void AddDelayOperation(EOGUIElementParentOperation<TElement> operation);

        object IList.this[int index]
        {
            get => AsList[index];
            set
            {
                AsList[index] = (TElement)value;
            }
        }

        void IList.Insert(int index, object value)
        {
            AsList.Insert(index, (TElement)value);
        }

        void IList.RemoveAt(int index)
        {
            AsList.RemoveAt(index);
        }

        int IList.Add(object value)
        {
            AsList.Add((TElement)value);
            return AsList.Count - 1;
        }

        void IList.Clear()
        {
            AsList.Clear();
        }

        void IList.Remove(object value)
        {
            AsList.Remove((TElement)value);
        }

        bool IList.Contains(object value)
        {
            return Contains((TElement)value);
        }

        int IList.IndexOf(object value)
        {
            return IndexOf((TElement)value);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            var target = (TElement[])array;
            AsList.CopyTo(target, index);
        }

        IEnumerator IEnumerable.GetEnumerator() => AsList.GetEnumerator();
    }

    public interface IEOGUIElementParent : IEOGUIElementChild, IList
    {
        protected static void SetChild(IEOGUIElementChild child, IEOGUIElementParent parent)
        {
            var oldParent = child.Parent;
            if (oldParent == parent) return;

            oldParent?.Remove(child);
            child.SetParent(parent);
        }

        bool IsHorizontal { get; }
    }
}