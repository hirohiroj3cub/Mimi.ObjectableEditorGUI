namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIRangeField : EOGUIRangeField<EOGUIRangeField>
    {
        public EOGUIRangeField(float defaultValue, float minLimit, float maxLimit) : base(defaultValue, minLimit, maxLimit)
        {
        }
    }

    public abstract class EOGUIRangeField<TSelf> : EOGUIRange<TSelf, EOGUIField<float>>
        where TSelf : EOGUIRangeField<TSelf>
    {
        protected EOGUIRangeField(float defaultValue, float minLimit, float maxLimit) : base(new EOGUIField<float>(defaultValue), minLimit, maxLimit)
        {
        }
    }
}