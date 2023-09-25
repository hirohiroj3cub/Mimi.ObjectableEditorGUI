namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUISpace : EOGUIElement<EOGUISpace>
    {
        public EOGUISpace(float size = 10)
        {
            Options = new EOGUIElementOptions<EOGUISpace>();
            Height = size;
        }

        public override EOGUIHeightType HeightType => EOGUIHeightType.Static;
        public float Height { get; set; }

        public override float GetElementHeight()
        {
            return Height;
        }

        public override void OnElementGUI()
        {
        }
    }
}