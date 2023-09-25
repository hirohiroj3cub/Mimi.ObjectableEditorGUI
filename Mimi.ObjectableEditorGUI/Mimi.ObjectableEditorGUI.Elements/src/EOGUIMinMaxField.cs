namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIMinMaxField : EOGUIMinMax<EOGUIMinMaxField, EOGUIField<float>>
    {
        public EOGUIMinMaxField(float defaultMin, float defaultMax, float minLimit, float maxLimit) :
            base(new EOGUIField<float>(defaultMin), new EOGUIField<float>(defaultMax), minLimit, maxLimit)
        {
        }
    }

    public abstract class EOGUIMinMaxField<TSelf> : EOGUIMinMax<TSelf, EOGUIField<float>>
        where TSelf : EOGUIMinMaxField<TSelf>
    {
        public EOGUIMinMaxField(float defaultMin, float defaultMax, float minLimit, float maxLimit) :
            base(new EOGUIField<float>(defaultMin), new EOGUIField<float>(defaultMax), minLimit, maxLimit)
        {

        }
    }
}