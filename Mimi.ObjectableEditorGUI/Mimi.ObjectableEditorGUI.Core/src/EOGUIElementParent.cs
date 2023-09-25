using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI
{

    public abstract class EOGUIElementParent<TSelf, TList, TElement> : EOGUIElement<TSelf>, IEOGUIElementParent<TSelf, TElement>, IList<TElement>, IList
        where TSelf : EOGUIElementParent<TSelf, TList, TElement>
        where TList : IList<TElement>, IList
        where TElement : IEOGUIElementChild
    {

        private readonly TList elements;
        private readonly IList<TElement> elementsT;
        private readonly Queue<EOGUIElementParentOperation<TElement>> delayOperations;

        protected TList Elements => elements;

        public bool IsHorizontal { get; }

        public int Count => elementsT.Count;

        public bool IsReadOnly => elementsT.IsReadOnly;

        public bool IsFixedSize => elements.IsFixedSize;

        public bool IsSynchronized => elements.IsSynchronized;

        public object SyncRoot => elements.SyncRoot;

        public EOGUIElementParent(TList elements, bool isHorizontal)
        {
            if (elements is null) throw new ArgumentNullException(nameof(elements));
            delayOperations = new Queue<EOGUIElementParentOperation<TElement>>();

            elementsT = this.elements = elements;
            foreach (var item in elementsT)
            {
                IEOGUIElementParent.SetChild(item, this);
            }

            IsHorizontal = isHorizontal;

            if (isHorizontal)
            {
                Options.Padding = new RectOffset(1, 1, 2, 2);
            }
            else
            {
                Options.Padding = new RectOffset(5, 5, 5, 3);
                Options.BackgroundStyle = GUI.skin.box;
            }
        }

        public void AddDelayOperation(EOGUIElementParentOperation<TElement> operation)
        {
            delayOperations.Enqueue(operation);
        }

        public void DelayAdd(Func<IEOGUIElementParent<TElement>, TElement> selector)
        {
            delayOperations.Enqueue(new EOGUIElementParentOperation<TElement>.Add(selector));
        }

        public void DelayInsert(Func<IEOGUIElementParent<TElement>, (int, TElement)> selector)
        {
            delayOperations.Enqueue(new EOGUIElementParentOperation<TElement>.Insert(selector));
        }

        public void DelayRemove(Func<IEOGUIElementParent<TElement>, TElement, bool> selector)
        {
            delayOperations.Enqueue(new EOGUIElementParentOperation<TElement>.Remove(selector));
        }

        public override void OnElementPreUpdate()
        {
            this.OnPreUpdateElements(Context);
        }

        public override float GetElementHeight()
        {
            return this.GetHeightElements();
        }

        public override void OnElementGUI()
        {
            this.OnGUIElements<TSelf>();
        }

        public override void EndElement()
        {
            base.EndElement();
            if (this.HasContextAndIsDirty()) throw new InvalidOperationException("EndElement status error. this.HasContextAndIsDirty() == true.");

            while (delayOperations.TryDequeue(out var operation))
            {
                operation.Invoke(ThisT);
            }
        }

        public virtual TElement this[int index]
        {
            get => elementsT[index];
            set
            {
                var old = elementsT[index];
                old?.SetParent(null);
                IEOGUIElementParent.SetChild(value, this);
                elementsT[index] = value;
                Context?.Drawer.SetFlagOfHeightUpdateAll();
            }
        }

        public virtual void Insert(int index, TElement item)
        {
            IEOGUIElementParent.SetChild(item, this);
            elementsT.Insert(index, item);
            Context?.Drawer.SetFlagOfHeightUpdateAll();
        }

        public virtual void RemoveAt(int index)
        {
            elementsT[index].SetParent(null);
            elementsT.RemoveAt(index);
            Context?.Drawer.SetFlagOfHeightUpdateAll();
        }

        public virtual void Add(TElement item)
        {
            elementsT.Add(item);
            IEOGUIElementParent.SetChild(item, this);
            Context?.Drawer.SetFlagOfHeightUpdateAll();
        }

        public virtual void Clear()
        {
            foreach (var item in this)
            {
                item.SetParent(null);
            }
            elementsT.Clear();
            Context?.Drawer.SetFlagOfHeightUpdateAll();
        }

        public virtual bool Remove(TElement item)
        {
            var oldParent = item.Parent;

            if (oldParent == this && elementsT.Remove(item))
            {
                item.SetParent(null);
                Context?.Drawer.SetFlagOfHeightUpdateAll();

                return true;
            }
            else
            {
                return false;
            }
        }

        public int IndexOf(TElement item)
        {
            return elements.IndexOf(item);
        }

        public bool Contains(TElement item)
        {
            return item.Parent == this;
        }

        public void CopyTo(TElement[] array, int arrayIndex)
        {
            elements.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return elements.GetEnumerator();
        }

    }
}