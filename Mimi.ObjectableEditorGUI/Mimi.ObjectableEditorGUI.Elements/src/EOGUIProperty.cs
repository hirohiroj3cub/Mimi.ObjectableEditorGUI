using Mimi.ObjectableEditorGUI.Context;
using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIProperty : EOGUIProperty<EOGUIProperty>
    {
        public EOGUIProperty() : this(new EOGUIContextWriterSerializedProperty())
        {
        }

        public EOGUIProperty(EOGUIContextWriterSerializedProperty serializedPropertySelector) : base(serializedPropertySelector)
        {
        }
    }

    public abstract class EOGUIProperty<TSelf> : EOGUIElement
        where TSelf : EOGUIProperty<TSelf>

    {
        protected EOGUIContextWriterSerializedProperty serializedPropertyWriter;

        protected EOGUIProperty(EOGUIContextWriterSerializedProperty serializedPropertyWriter)
        {
            Options = new EOGUIElementOptions<TSelf>()
            {
                Margin = new RectOffset(1, 1, 0, 2),
            };

            this.serializedPropertyWriter = serializedPropertyWriter ?? new EOGUIContextWriterSerializedProperty();
        }

        public override EOGUIHeightType HeightType => EOGUIHeightType.Animation;
        public new EOGUIElementOptions<TSelf> Options { get; set; }
        public sealed override EOGUIElementOptions GetOptions() => Options;

        public override void OnElementContextUpdate()
        {
            base.OnElementContextUpdate();
            serializedPropertyWriter.Write(this, Status);
            UpdateSerializedProperty();
        }

        public override float GetElementHeight()
        {
            if (SerializedProperty.hasVisibleChildren && !SerializedProperty.isExpanded)
            {
                return EditorStyles.foldoutPreDrop.CalcSize(ValidLabel).y;
            }
            else
            {
                return EditorGUI.GetPropertyHeight(SerializedProperty, ValidLabel);
            }
        }

        public override void OnElementGUI()
        {
            var label = ValidLabel;
            ref readonly var rect = ref Status.rect;

            SerializedProperty.serializedObject.Update();

            float height;
            if (SerializedProperty.hasVisibleChildren)
            {
                EditorGUI.PropertyField(rect, SerializedProperty, label, true);
                if (SerializedProperty.isExpanded)
                {
                    height = EditorGUI.GetPropertyHeight(SerializedProperty, label);
                }
                else
                {
                    height = EditorStyles.foldoutPreDrop.CalcSize(label).y;
                }
            }
            else
            {
                EditorGUI.PropertyField(rect, SerializedProperty, label);
                height = EditorGUI.GetPropertyHeight(SerializedProperty, label);
            }

            if (SerializedProperty.serializedObject.ApplyModifiedProperties() ||
                height != ((IEOGUIElement)this).ExtensionsResource.cachedHeight.target)
            {
                Context.Drawer.SetFlagOfHeightUpdateAll();
            }
        }
    }
}