using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIReorderablePropertyFieldElement<T> : EOGUIReorderablePropertyFieldElement<EOGUIReorderablePropertyFieldElement<T>, T>
    {

    }

    public abstract class EOGUIReorderablePropertyFieldElement<TSelf, T> : EOGUIPropertyField<TSelf, T>
        where TSelf : EOGUIReorderablePropertyFieldElement<TSelf, T>
    {
        public EOGUIReorderablePropertyFieldElement() : base(new ObjectableEditorGUI.Context.EOGUIContextWriterSerializedProperty())
        {
        }

        public override void OnElementContextUpdate()
        {
            base.OnElementContextUpdate();

            if (Index < Status.SerializedProperty.arraySize)
            {
                Status.SerializedProperty = Status.SerializedProperty.GetArrayElementAtIndex(Index);
                UpdateSerializedProperty();
            }
        }

        public override void OnElementGUI()
        {
            var indexRect = new Rect(Status.rect.x, Status.rect.y - 2, 40, EditorGUIUtility.singleLineHeight);
            var label = EditorGUI.BeginProperty(indexRect, null, SerializedProperty);
            label.text = $"[{Index}]";
            GUI.Label(indexRect, label);
            EditorGUI.EndProperty();

            Status.rect.xMin += indexRect.width;

            EOGUIReorderableUtility.LabelTextVaild(ValidLabel, Index, Value);
            base.OnElementGUI();
        }
    }
}