using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public class EOGUIPopup<T> : IDictionary<string, T>
    {
        private GUIContent[] contents;

        public T this[string key] { get => ((IDictionary<string, T>)PopupMenu)[key]; set => ((IDictionary<string, T>)PopupMenu)[key] = value; }

        public Dictionary<string, T> PopupMenu { get; } = new Dictionary<string, T>(StringComparer.Ordinal);

        public ICollection<string> Keys => ((IDictionary<string, T>)PopupMenu).Keys;

        public ICollection<T> Values => ((IDictionary<string, T>)PopupMenu).Values;

        public int Count => ((ICollection<KeyValuePair<string, T>>)PopupMenu).Count;

        bool ICollection<KeyValuePair<string, T>>.IsReadOnly => ((ICollection<KeyValuePair<string, T>>)PopupMenu).IsReadOnly;

        public float GetLineHeight(IEOGUIElement e)
        {
            return EditorStyles.popup.CalcSize(e.ValidLabel).y;
        }

        public T OnLineGUI(IEOGUIElement e, T value)
        {
            var count = PopupMenu.Count;

            if (contents == null || contents.Length != count)
            {
                contents = new GUIContent[count];
                for (int i = 0; i < contents.Length; i++)
                {
                    contents[i] = new GUIContent();
                }
            }

            var currentValue = value;
            int currentIndex = -1;
            for (int i = 0; i < count; i++)
            {
                var p = PopupMenu.ElementAt(i);
                contents[i].text = p.Key;
                if (p.Value.Equals(currentValue))
                {
                    currentIndex = i;
                }
            }

            var newIndex = EditorGUI.Popup(e.Status.rect, e.ValidLabel, currentIndex, contents);

            if (newIndex >= 0 && newIndex != currentIndex)
            {
                value = PopupMenu.ElementAt(newIndex).Value;
            }

            return value;
        }

        public void Add(string key, T value)
        {
            ((IDictionary<string, T>)PopupMenu).Add(key, value);
        }

        public bool Remove(string key)
        {
            return ((IDictionary<string, T>)PopupMenu).Remove(key);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<string, T>>)PopupMenu).Clear();
        }

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, T>)PopupMenu).ContainsKey(key);
        }

        public bool TryGetValue(string key, out T value)
        {
            return ((IDictionary<string, T>)PopupMenu).TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, T>>)PopupMenu).GetEnumerator();
        }

        void ICollection<KeyValuePair<string, T>>.Add(KeyValuePair<string, T> item)
        {
            ((ICollection<KeyValuePair<string, T>>)PopupMenu).Add(item);
        }

        bool ICollection<KeyValuePair<string, T>>.Remove(KeyValuePair<string, T> item)
        {
            return ((ICollection<KeyValuePair<string, T>>)PopupMenu).Remove(item);
        }

        bool ICollection<KeyValuePair<string, T>>.Contains(KeyValuePair<string, T> item)
        {
            return ((ICollection<KeyValuePair<string, T>>)PopupMenu).Contains(item);
        }

        void ICollection<KeyValuePair<string, T>>.CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, T>>)PopupMenu).CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)PopupMenu).GetEnumerator();
        }
    }
}