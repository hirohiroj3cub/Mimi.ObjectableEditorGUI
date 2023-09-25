namespace Mimi.ObjectableEditorGUI
{
    public interface IEOGUIElementChild : IEOGUIElement
    {
        void SetParent(IEOGUIElementParent parent);
    }
}