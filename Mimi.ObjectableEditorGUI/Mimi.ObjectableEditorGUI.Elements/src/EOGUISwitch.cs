using System.Collections.Generic;

namespace Mimi.ObjectableEditorGUI.Elements
{

    public sealed class EOGUISwitch<T> : EOGUIElementWraper<EOGUISwitch<T>, IEOGUIElementChild>, IDictionary<T, IEOGUIElementChild>
    {
        private readonly Dictionary<T, IEOGUIElementChild> children;

        public EOGUISwitch(T defaultKey, IEOGUIElementChild defaultElement) : base(defaultElement)
        {
            children = new Dictionary<T, IEOGUIElementChild>()
            {
                {defaultKey, defaultElement }
            };
            DefaultElement = defaultElement;
        }

        public void DelaySetKey(T key)
        {
            DelaySet(e =>
            {
                Key = key;
                if (!children.TryGetValue(key, out IEOGUIElementChild child))
                {
                    child = DefaultElement;
                }
                return child;
            });
        }

        public IEOGUIElementChild SetKey(T key)
        {
            Key = key;
            if (!children.TryGetValue(key, out IEOGUIElementChild child))
            {
                child = DefaultElement;
            }

            Element = child;
            return child;
        }

        public T Key { get; private set; }
        public IEOGUIElementChild DefaultElement { get; }

        public IEOGUIElementChild this[T key] { get => ((IDictionary<T, IEOGUIElementChild>)children)[key]; set => ((IDictionary<T, IEOGUIElementChild>)children)[key] = value; }

        public ICollection<T> Keys => ((IDictionary<T, IEOGUIElementChild>)children).Keys;

        public ICollection<IEOGUIElementChild> Values => ((IDictionary<T, IEOGUIElementChild>)children).Values;

        public int Count => ((ICollection<KeyValuePair<T, IEOGUIElementChild>>)children).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<T, IEOGUIElementChild>>)children).IsReadOnly;

        public void Add(T key, IEOGUIElementChild value)
        {
            ((IDictionary<T, IEOGUIElementChild>)children).Add(key, value);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<T, IEOGUIElementChild>>)children).Clear();
        }

        public bool ContainsKey(T key)
        {
            return ((IDictionary<T, IEOGUIElementChild>)children).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<T, IEOGUIElementChild>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<T, IEOGUIElementChild>>)children).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<T, IEOGUIElementChild>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<T, IEOGUIElementChild>>)children).GetEnumerator();
        }

        public bool Remove(T key)
        {
            return ((IDictionary<T, IEOGUIElementChild>)children).Remove(key);
        }

        public bool TryGetValue(T key, out IEOGUIElementChild value)
        {
            return ((IDictionary<T, IEOGUIElementChild>)children).TryGetValue(key, out value);
        }

        void ICollection<KeyValuePair<T, IEOGUIElementChild>>.Add(KeyValuePair<T, IEOGUIElementChild> item)
        {
            ((ICollection<KeyValuePair<T, IEOGUIElementChild>>)children).Add(item);
        }

        bool ICollection<KeyValuePair<T, IEOGUIElementChild>>.Contains(KeyValuePair<T, IEOGUIElementChild> item)
        {
            return ((ICollection<KeyValuePair<T, IEOGUIElementChild>>)children).Contains(item);
        }

        bool ICollection<KeyValuePair<T, IEOGUIElementChild>>.Remove(KeyValuePair<T, IEOGUIElementChild> item)
        {
            return ((ICollection<KeyValuePair<T, IEOGUIElementChild>>)children).Remove(item);
        }
    }
}