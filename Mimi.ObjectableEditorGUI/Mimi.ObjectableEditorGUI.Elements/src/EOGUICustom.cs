namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUICustom : EOGUIElement<EOGUICustom>
    {
        public EOGUIElementEventHandler<EOGUICustom> OnElementContextUpdateCustom { get; set; }
        public EOGUIElementEventHandler<EOGUICustom> OnElementPreUpdateCustom { get; set; }
        public EOGUIElementEventFuncHandler<EOGUICustom, float> GetElementHeightCustom { get; set; }
        public EOGUIElementEventHandler<EOGUICustom> OnElementGUICustom { get; set; }

        public EOGUIHeightType HeightTypeCustom { get; set; } = EOGUIHeightType.Static;
        public override EOGUIHeightType HeightType => HeightTypeCustom;

        public override void OnElementContextUpdate()
        {
            base.OnElementContextUpdate();
            OnElementContextUpdateCustom?.Invoke(ThisT);
        }

        public override void OnElementPreUpdate()
        {
            OnElementPreUpdateCustom?.Invoke(ThisT);
        }

        public override float GetElementHeight()
        {
            return GetElementHeightCustom?.Invoke(ThisT) ?? 0f;
        }

        public override void OnElementGUI()
        {
            OnElementGUICustom?.Invoke(ThisT);
        }
    }
}