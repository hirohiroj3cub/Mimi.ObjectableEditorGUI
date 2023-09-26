using Mimi.ObjectableEditorGUI.Context;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIRangePropertyField : EOGUIRangePropertyField<EOGUIRangePropertyField>
    {
        public EOGUIRangePropertyField(EOGUIContextWriterSerializedProperty serializedPropertySelector, float minLimit, float maxLimit) : base(serializedPropertySelector, minLimit, maxLimit)
        {
        }
    }

    public abstract class EOGUIRangePropertyField<TSelf> : EOGUIRange<TSelf, EOGUIPropertyField<float>>
        where TSelf : EOGUIRangePropertyField<TSelf>
    {
        protected EOGUIRangePropertyField(EOGUIContextWriterSerializedProperty serializedPropertySelector, float minLimit, float maxLimit) :
            base(new EOGUIPropertyField<float>(serializedPropertySelector, Mathf.Clamp(0f, minLimit, maxLimit)), minLimit, maxLimit)
        {
        }
    }
}