using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Mimi.ObjectableEditorGUI
{
    public class EOGUIElementChildrenValueList<TParent, TElement, T> : IList<T>, IList
        where TParent : class, IEOGUIElementParent<TElement>
        where TElement : IEOGUIElementChild
    {
        private readonly WeakReference<TParent> owner;

        public TParent Owner
        {
            get
            {
                if (owner.TryGetTarget(out var ret)) return ret;
                CreateNewElement = null;
                GetValue = null;
                SetValue = null;
                return null;
            }
        }

        public IList<TElement> OwnerAsList => Owner;

        public Func<TElement> CreateNewElement { get; private set; }

        public Func<TElement, T> GetValue { get; private set; }

        public Action<TElement, T> SetValue { get; private set; }

        public EOGUIElementChildrenValueList(TParent owner, Func<TElement> createNewElement, Func<TElement, T> getValue, Action<TElement, T> setValue)
        {
            this.owner = new WeakReference<TParent>(owner ?? throw new ArgumentNullException(nameof(owner)));
            CreateNewElement = createNewElement ?? throw new ArgumentNullException(nameof(createNewElement));
            GetValue = getValue ?? throw new ArgumentNullException(nameof(getValue));
            SetValue = setValue ?? throw new ArgumentNullException(nameof(setValue));
        }

        public T this[int index] { get => GetValue(OwnerAsList[index]); set => SetValue(OwnerAsList[index], value); }

        object IList.this[int index] { get => this[index]; set => this[index] = (T)value; }

        public int Count => OwnerAsList.Count;

        public bool IsReadOnly => false;

        bool IList.IsFixedSize => false;

        bool ICollection.IsSynchronized => Owner.IsSynchronized;

        object ICollection.SyncRoot => Owner.SyncRoot;

        public void Add(T value)
        {
            var e = CreateNewElement();
            SetValue(e, value);
            Owner.Add(e);
        }

        public void Clear()
        {
            OwnerAsList.Clear();
        }

        public bool Contains(T value)
        {
            for (int i = 0; i < OwnerAsList.Count; i++)
            {
                if (GetValue(OwnerAsList[i]).Equals(value)) return true;
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Owner.Select(e => GetValue(e)).ToArray().CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Owner.Select(e => GetValue(e)).GetEnumerator();
        }

        public int IndexOf(T value)
        {
            for (int i = 0; i < OwnerAsList.Count; i++)
            {
                if (GetValue(OwnerAsList[i]).Equals(value)) return i;
            }

            return -1;
        }

        public void Insert(int index, T value)
        {
            var values = Owner.Select(e => GetValue(e)).ToList();
            Owner.Add(CreateNewElement());
            values.Insert(index, value);
            for (int i = index; i < values.Count; i++)
            {
                SetValue(OwnerAsList[i], values[i]);
            }
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                OwnerAsList.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveAt(int index)
        {
            OwnerAsList.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        int IList.Add(object value)
        {
            if (value is T t)
            {
                Add(t);
                return Count - 1;
            }
            return -1;
        }

        bool IList.Contains(object value)
        {
            if (value is T t)
            {
                return Contains(t);
            }
            else
            {
                return false;
            }
        }

        int IList.IndexOf(object value)
        {
            if (value is T t)
            {
                return IndexOf(t);
            }
            else
            {
                return -1;
            }
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, (T)value);
        }

        void IList.Remove(object value)
        {
            Remove((T)value);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            var arrayT = (T[])array;
            CopyTo(arrayT, index);
        }

        public void DelayAdd(T value)
        {
            Owner.DelayAdd(self =>
            {
                var element = CreateNewElement();
                SetValue(element, value);
                return element;
            });
        }

        public void DelayInsert(int index, T value)
        {
            Owner.DelayInsert(self =>
            {
                var element = CreateNewElement();
                SetValue(element, value);
                return (index, element);
            });
        }

        public void DelayRemove(T value)
        {
            Owner.DelayRemove((self, e) =>
            {
                return GetValue(e).Equals(value);
            });
        }

        public void DelayRemoveAt(int index)
        {
            Owner.DelayRemove((self, e) =>
            {
                return e.Index == index;
            });
        }
    }
}