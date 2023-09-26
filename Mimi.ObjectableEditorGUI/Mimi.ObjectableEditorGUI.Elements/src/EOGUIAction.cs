namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIAction : EOGUIElement<EOGUIAction>
    {
        public EOGUIAction()
        {
            Options = new EOGUIElementOptions<EOGUIAction>();
        }

        public override EOGUIHeightType HeightType => EOGUIHeightType.Static;
        public EOGUIElementEventHandler<EOGUIAction>? OnElementContextUpdateCustom { get; set; }
        public EOGUIElementEventHandler<EOGUIAction>? OnElementPreUpdateCustom { get; set; }
        public EOGUIElementEventHandler<EOGUIAction>? OnElementGUICustom { get; set; }

        public override void OnElementContextUpdate()
        {
            base.OnElementContextUpdate();
            OnElementContextUpdateCustom?.Invoke(ThisT);
        }

        public override void OnElementPreUpdate()
        {
            OnElementPreUpdateCustom?.Invoke(ThisT);
        }

        public override float GetElementHeight() => 0f;

        public override void OnElementGUI()
        {
            OnElementGUICustom?.Invoke(ThisT);
        }
    }
}