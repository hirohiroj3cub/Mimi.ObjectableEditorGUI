using System;
using System.Collections.Generic;

namespace Mimi.ObjectableEditorGUI.Context
{
    public delegate void EOGUIContextWriterAction(IEOGUIElement element, EOGUIContextStatus status);

    public interface IEOGUIContextWriterReadonly : IEnumerable<EOGUIContextWriterAction>
    {
        ReadOnlySpan<EOGUIContextWriterAction> Updaters { get; }
    }

    public interface IEOGUIContextWriterReadonly<TParam> : IEOGUIContextWriterReadonly
        where TParam : IEOGUIContextParam
    {
    }

    public static class IEOGUIContextWriterReadonly_Extensions
    {
        public static void Write(this IEOGUIContextWriterReadonly writer, IEOGUIElement element, EOGUIContextStatus status)
        {
            foreach (var updater in writer.Updaters)
            {
                updater?.Invoke(element, status);
            }
        }
    }
}