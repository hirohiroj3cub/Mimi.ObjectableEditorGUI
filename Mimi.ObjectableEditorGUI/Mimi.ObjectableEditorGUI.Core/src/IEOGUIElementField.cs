using System;

namespace Mimi.ObjectableEditorGUI
{

    public interface IEOGUIElementField<TSelf, TValue> : IEOGUIElementField<TValue>, IEOGUIElement<TSelf>
        where TSelf : class, IEOGUIElementField<TSelf, TValue>
    {
    }

    public interface IEOGUIElementField<TValue> : IEOGUIElementChild
    {
        event Action<TValue> OnChangedValue;
        TValue Value { get; set; }
    }
}