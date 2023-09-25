using Mimi.ObjectableEditorGUI.Context;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIMinMaxPropertyField : EOGUIMinMaxPropertyField<EOGUIMinMaxPropertyField>
    {
        public EOGUIMinMaxPropertyField(EOGUIContextWriterSerializedProperty minProperty, EOGUIContextWriterSerializedProperty maxProperty, float minLimit, float maxLimit) : base(minProperty, maxProperty, minLimit, maxLimit)
        {
        }
    }

    public abstract class EOGUIMinMaxPropertyField<TSelf> : EOGUIMinMax<EOGUIMinMaxPropertyField<TSelf>, EOGUIPropertyField<float>>
        where TSelf : EOGUIMinMaxPropertyField<TSelf>
    {
        public EOGUIMinMaxPropertyField(EOGUIContextWriterSerializedProperty minProperty, EOGUIContextWriterSerializedProperty maxProperty, float minLimit, float maxLimit)
            : base(new EOGUIPropertyField<float>(minProperty), new EOGUIPropertyField<float>(maxProperty), minLimit, maxLimit)
        {
        }
    }
}