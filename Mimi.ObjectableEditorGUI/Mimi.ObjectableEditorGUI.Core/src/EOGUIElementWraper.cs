using Mimi.ObjectableEditorGUI.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mimi.ObjectableEditorGUI
{
    public sealed class EOGUIElementWraper<TElement> : EOGUIElementWraper<EOGUIElementWraper<TElement>, TElement>
        where TElement : class, IEOGUIElementChild

    {
        public EOGUIElementWraper(TElement element) : base(element)
        {
        }
    }

    public abstract class EOGUIElementWraper<TSelf, TElement> : EOGUIElement<TSelf>,
        IEOGUIElementParent<TSelf, TElement>, IEOGUIElementParent, IEOGUIElement, IList<TElement>, IList
        where TSelf : EOGUIElementWraper<TSelf, TElement>
        where TElement : class, IEOGUIElementChild
    {
        private class SetOperations : EOGUIElementParentOperation<TElement>
        {
            private readonly Func<IEOGUIElementParent<TElement>, TElement> selector;

            public SetOperations(Func<IEOGUIElementParent<TElement>, TElement> selector)
            {
                this.selector = selector;
            }

            public override void Invoke(IEOGUIElementParent<TElement> self)
            {
                (self as EOGUIElementWraper<TSelf, TElement>).Element = selector(self);
            }
        }

        private readonly EOGUISpace empty = new EOGUISpace(0) { Options = { Visible = false } };
        private readonly TElement[] elements;
        private readonly ReadOnlyCollection<TElement> readOnlyCollection;
        private readonly Queue<EOGUIElementParentOperation<TElement>> delayOperations;

        public TElement Element
        {
            get
            {
                return elements[0];
            }
            set
            {
                ref var element = ref elements[0];
                if (!Equals(element, value))
                {
                    IEOGUIElementParent.SetChild(value, this);
                    element = value;
                    Context?.Drawer.SetFlagOfHeightUpdateAll();
                }
            }
        }

        private IEOGUIElement AsIElement
        {
            get
            {
                var e = Element;
                if (e == null) return empty;
                else return e;
            }
        }

        public override EOGUIHeightType HeightType => EOGUIHeightType.Animation;

        protected EOGUIElementWraper(TElement element)
        {
            elements = new TElement[1] { element };
            IEOGUIElementParent.SetChild(element, this);
            readOnlyCollection = new ReadOnlyCollection<TElement>(elements);
            delayOperations = new Queue<EOGUIElementParentOperation<TElement>>();
        }

        public override void OnElementContextUpdate()
        {
            base.OnElementContextUpdate();
            //AsIElement.OnElementContextUpdate();
        }

        public override void OnElementPreUpdate()
        {
            AsIElement.OnPreUpdateProperty(Context);
        }

        public override float GetElementHeight()
        {
            return AsIElement.GetHeightProperty();
        }

        public override void OnElementGUI()
        {
            AsIElement.OnGUIProperty();
        }

        public override void EndElement()
        {
            base.EndElement();

            while (delayOperations.TryDequeue(out var operation))
            {
                operation.Invoke(ThisT);
            }
        }

        public void AddDelayOperation(EOGUIElementParentOperation<TElement> operation)
        {
            delayOperations.Enqueue(operation);
        }

        public void DelaySet(Func<IEOGUIElementParent<TElement>, TElement> element)
        {
            delayOperations.Enqueue(new SetOperations(element));
        }

        bool IEOGUIElementParent.IsHorizontal => false;

        void IEOGUIElementParent<TElement>.DelayAdd(Func<IEOGUIElementParent<TElement>, TElement> element)
        {
            throw new NotSupportedException();
        }

        void IEOGUIElementParent<TElement>.DelayInsert(Func<IEOGUIElementParent<TElement>, (int, TElement)> element)
        {
            throw new NotSupportedException();
        }

        void IEOGUIElementParent<TElement>.DelayRemove(Func<IEOGUIElementParent<TElement>, TElement, bool> selector)
        {
            delayOperations.Enqueue(new EOGUIElementParentOperation<TElement>.Remove(selector));
        }

        #region IList, IList<T>

        TElement IList<TElement>.this[int index] { get => ((IList<TElement>)readOnlyCollection)[index]; set => ((IList<TElement>)readOnlyCollection)[index] = value; }

        int ICollection<TElement>.Count => ((ICollection<TElement>)readOnlyCollection).Count;

        bool ICollection<TElement>.IsReadOnly => ((ICollection<TElement>)readOnlyCollection).IsReadOnly;

        bool IList.IsFixedSize => ((IList)readOnlyCollection).IsFixedSize;

        bool IList.IsReadOnly => ((IList)readOnlyCollection).IsReadOnly;

        int ICollection.Count => ((ICollection)readOnlyCollection).Count;

        bool ICollection.IsSynchronized => ((ICollection)readOnlyCollection).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)readOnlyCollection).SyncRoot;

        void ICollection<TElement>.Add(TElement item)
        {
            ((ICollection<TElement>)readOnlyCollection).Add(item);
        }

        void ICollection<TElement>.Clear()
        {
            ((ICollection<TElement>)readOnlyCollection).Clear();
        }

        bool ICollection<TElement>.Contains(TElement item)
        {
            return ((ICollection<TElement>)readOnlyCollection).Contains(item);
        }

        void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex)
        {
            ((ICollection<TElement>)readOnlyCollection).CopyTo(array, arrayIndex);
        }

        IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
        {
            return ((IEnumerable<TElement>)readOnlyCollection).GetEnumerator();
        }

        int IList<TElement>.IndexOf(TElement item)
        {
            return ((IList<TElement>)readOnlyCollection).IndexOf(item);
        }

        void IList<TElement>.Insert(int index, TElement item)
        {
            ((IList<TElement>)readOnlyCollection).Insert(index, item);
        }

        bool ICollection<TElement>.Remove(TElement item)
        {
            if (item.Parent == this && elements[0].Equals(item))
            {
                item.SetParent(null);
                elements[0] = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        void IList<TElement>.RemoveAt(int index)
        {
            if (index == 0)
            {

            }
            ((IList<TElement>)readOnlyCollection).RemoveAt(index);
        }

        #endregion
    }
}