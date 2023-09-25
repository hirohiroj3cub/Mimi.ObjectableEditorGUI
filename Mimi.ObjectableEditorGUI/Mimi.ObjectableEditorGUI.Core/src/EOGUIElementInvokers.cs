namespace Mimi.ObjectableEditorGUI
{
    public static class EOGUIElementInvokers
    {
        public class ContextUpdate : EOGUIElementInvoker<ContextUpdate>
        {
            protected override void Invoke_Protected()
            {
                Element.OnElementContextUpdate();
            }
        }

        public class PreUpdate : EOGUIElementInvoker<PreUpdate>
        {
            protected override void Invoke_Protected()
            {
                Element.OnElementPreUpdate();
            }
        }

        public class GUI : EOGUIElementInvoker<GUI>
        {
            protected override void Invoke_Protected()
            {
                Element.OnElementGUI();
            }
        }
    }
}