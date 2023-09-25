using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mimi.ObjectableEditorGUI.Context
{
    public class EOGUIContextWriterActive<TParam, TWriter> : IEOGUIContextWriterReadonly<TParam>
        where TParam : IEOGUIContextParam
        where TWriter : IEOGUIContextWriterReadonly<TParam>
    {
        private readonly EOGUIContextWriterAction[] updaters;

        public TWriter Writer { get; }

        public EOGUIContextWriterActive(TWriter writer)
        {
            Writer = writer;
            updaters = new EOGUIContextWriterAction[1]
            {
                Updater,
            };
        }

        public ReadOnlySpan<EOGUIContextWriterAction> Updaters => new ReadOnlySpan<EOGUIContextWriterAction>(updaters);

        private void Updater(IEOGUIElement element, EOGUIContextStatus status)
        {
            foreach (var item in Writer.Updaters)
            {
                item?.Invoke(element, status);
            }
        }

        public IEnumerator<EOGUIContextWriterAction> GetEnumerator()
        {
            return updaters.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return updaters.GetEnumerator();
        }
    }
}