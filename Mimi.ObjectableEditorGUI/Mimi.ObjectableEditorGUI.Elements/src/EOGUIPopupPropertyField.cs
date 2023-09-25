using Mimi.ObjectableEditorGUI.Context;
using UnityEditor;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIPopupPropertyField<T> : EOGUIPopupPropertyField<EOGUIPopupPropertyField<T>, T>
    {
        public EOGUIPopupPropertyField(EOGUIContextWriterSerializedProperty serializedPropertySelector) : base(serializedPropertySelector)
        {
        }
    }

    public abstract class EOGUIPopupPropertyField<TSelf, T> : EOGUIPropertyField<TSelf, T>, IEOGUIElementField<TSelf, T>
        where TSelf : EOGUIPopupPropertyField<TSelf, T>
    {
        public override EOGUIHeightType HeightType => EOGUIHeightType.Static;
        public EOGUIPopup<T> Popup { get; }

        protected EOGUIPopupPropertyField(EOGUIContextWriterSerializedProperty serializedPropertySelector) : base(serializedPropertySelector)
        {
            Popup = new EOGUIPopup<T>();
        }

        public override float GetElementHeight()
        {
            return Popup.GetLineHeight(this);
        }

        public override void OnElementGUI()
        {
            EditorGUI.BeginChangeCheck();

            var newValue = Popup.OnLineGUI(this, Value);

            if (EditorGUI.EndChangeCheck())
            {
                Value = newValue;
            }
        }
    }
}