namespace Mimi.ObjectableEditorGUI
{
    public delegate void EOGUIElementEventHandler<in TElement>(TElement element) where TElement : IEOGUIElement;

    public delegate void EOGUIElementEventHandler<in TElement, in TArg>(TElement element, TArg arg) where TElement : IEOGUIElement;

    public delegate TRet EOGUIElementEventFuncHandler<in TElement, out TRet>(TElement element) where TElement : IEOGUIElement;

    public delegate TRet EOGUIElementEventFuncHandler<in TElement, in TArg, out TRet>(TElement element, TArg arg) where TElement : IEOGUIElement;
}
