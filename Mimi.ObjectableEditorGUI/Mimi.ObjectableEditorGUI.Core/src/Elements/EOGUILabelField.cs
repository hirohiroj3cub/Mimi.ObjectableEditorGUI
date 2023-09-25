using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUILabelField : EOGUILabelField<EOGUILabelField>
    {
        public EOGUILabelField() : base()
        {
        }

        public EOGUILabelField(GUIContent mainLabel) : base(mainLabel)
        {
        }

        public EOGUILabelField(string mainLabelText) : base(mainLabelText)
        {
        }

        public EOGUILabelField(GUIContent mainLabel, GUIContent subLabel) : base(mainLabel, subLabel)
        {
        }

        public EOGUILabelField(string mainLabelText, string subLabelText) : base(mainLabelText, subLabelText)
        {
        }
    }

    public abstract class EOGUILabelField<TSelf> : EOGUIElement<TSelf>
        where TSelf : EOGUILabelField<TSelf>
    {
        public GUIContent SubLabel { get; set; }

        public EOGUILabelField(GUIContent mainLabel, GUIContent subLabel)
        {
            SubLabel = subLabel;
            Label = mainLabel;
        }

        public EOGUILabelField(GUIContent subLabel) : this(null, subLabel)
        {
        }

        public EOGUILabelField() : this((GUIContent)null, null)
        {
        }

        public EOGUILabelField(string mainLabelText) : this(new GUIContent(mainLabelText), null)
        {
        }

        public EOGUILabelField(string mainLabelText, string subLabelText) : this(new GUIContent(mainLabelText), new GUIContent(subLabelText))
        {
        }

        public override EOGUIHeightType HeightType => EOGUIHeightType.Static;

        public override float GetElementHeight()
        {
            return EditorStyles.label.CalcSize(ValidLabel).y;
        }

        public override void OnElementGUI()
        {
            if (SubLabel == null)
            {
                EditorGUI.LabelField(Status.rect, ValidLabel);
            }
            else
            {
                EditorGUI.LabelField(Status.rect, ValidLabel, SubLabel);
            }
        }
    }
}