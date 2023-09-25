using System;
using System.Collections;
using System.Collections.Generic;

namespace Mimi.ObjectableEditorGUI.Context
{
    public abstract class EOGUIContextWriter<TParam> : IEOGUIContextWriter<TParam>
        where TParam : IEOGUIContextParam
    {
        private readonly List<EOGUIContextWriterAction> updatersList;
        private EOGUIContextWriterAction[] updatersArray;
        private readonly List<string> keys;
        private bool arrayUpdate = false;

        public ReadOnlySpan<EOGUIContextWriterAction> Updaters
        {
            get
            {
                if (arrayUpdate)
                {
                    updatersArray = updatersList.ToArray();
                    arrayUpdate = false;
                }
                return new ReadOnlySpan<EOGUIContextWriterAction>(updatersArray);
            }
        }

        public EOGUIContextWriter()
        {
            updatersList = new List<EOGUIContextWriterAction>();
            updatersArray = updatersList.ToArray();
            keys = new List<string>();
        }

        public void Add<T>(T param, string key = "") where T : struct, TParam
        {
            Insert(param, updatersList.Count, key);
        }

        public void Add<T>(IEOGUIContextWriterReadonly<T> other, string key = "") where T : TParam
        {
            Insert(other, updatersList.Count, key);
        }

        public void AddHead<T>(T param, string key = "") where T : struct, TParam
        {
            Insert(param, 0, key);
        }

        public void AddHead<T>(IEOGUIContextWriterReadonly<T> other, string key = "") where T : TParam
        {
            Insert(other, 0, key);
        }

        private void Insert<T>(in T param, int index, string key = "") where T : struct, TParam
        {
            updatersList.Insert(index, param.Updater);
            arrayUpdate = true;
            keys.Insert(index, key);
        }

        private void Insert<T>(IEOGUIContextWriterReadonly<T> other, int index, string key = "") where T : TParam
        {
            var count = other?.Updaters.Length ?? 0;
            if (count > 0)
            {
                var newKeys = new string[count];
                Array.Fill(newKeys, key);
                keys.InsertRange(index, newKeys);

                updatersList.Capacity = updatersList.Count + count;
                foreach (var item in other.Updaters)
                {
                    updatersList.Insert(index++, item);
                }
                arrayUpdate = true;
            }
        }

        public bool Remove(string key = "")
        {
            int index = 0;
            bool find = false;
            while (true)
            {
                var newIndex = keys.IndexOf(key, index);
                if (newIndex >= 0)
                {
                    updatersList.RemoveAt(newIndex);
                    keys.RemoveAt(newIndex);
                    index = newIndex;
                    find |= true;
                    continue;
                }
                else
                {
                    break;
                }
            }

            arrayUpdate |= find;
            return find;
        }

        public bool RemoveAll(string[] keys)
        {
            bool removed = false;

            for (int i = 0; i < keys.Length; i++)
            {
                removed |= Remove(keys[i]);
            }

            return removed;
        }

        public void Clear()
        {
            if (updatersList.Count > 0)
            {
                updatersList.Clear();
                arrayUpdate = true;
                keys.Clear();
            }
        }



        public IEnumerator<EOGUIContextWriterAction> GetEnumerator()
        {
            return updatersList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)updatersList).GetEnumerator();
        }
    }
}