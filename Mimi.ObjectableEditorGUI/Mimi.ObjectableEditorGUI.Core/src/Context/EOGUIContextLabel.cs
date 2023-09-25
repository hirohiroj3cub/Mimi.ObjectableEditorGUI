using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Context
{
    public static class EOGUIContextLabel
    {
        public static readonly SetPropertyName PropertyName;
        public static readonly SetElementName ElementName;
        public static readonly SetParentElementName ParentElementName;

        public static SetValue Value(GUIContent label) => new SetValue(label);
        public static SetText Text(string text) => new SetText(text);

        static EOGUIContextLabel()
        {
            PropertyName = new SetPropertyName();
            ElementName = new SetElementName();
            ParentElementName = new SetParentElementName();
        }

        public readonly struct SetPropertyName : IEOGUIContextLabel
        {
            private static readonly EOGUIContextWriterAction updater;

            static SetPropertyName()
            {
                var label = new GUIContent();
                updater = (IEOGUIElement element, EOGUIContextStatus status) =>
                {
                    label.text = status.SerializedProperty.displayName;
                    status.label = label;
                };
            }

            public EOGUIContextWriterAction Updater => updater;
        }

        public readonly struct SetElementName : IEOGUIContextLabel
        {
            private static readonly EOGUIContextWriterAction updater;

            static SetElementName()
            {
                var label = new GUIContent();
                updater = (IEOGUIElement element, EOGUIContextStatus status) =>
                {
                    label.text = ObjectNames.NicifyVariableName(element.Name);
                    status.label = label;
                };
            }

            public EOGUIContextWriterAction Updater => updater;
        }

        public readonly struct SetParentElementName : IEOGUIContextLabel
        {
            private static readonly EOGUIContextWriterAction updater;

            static SetParentElementName()
            {
                var label = new GUIContent();
                updater = (IEOGUIElement element, EOGUIContextStatus status) =>
                {
                    label.text = ObjectNames.NicifyVariableName(element.Parent?.Name ?? "Parent is null");
                    status.label = label;
                };
            }

            public EOGUIContextWriterAction Updater => updater;
        }

        public readonly struct SetValue : IEOGUIContextLabel
        {
            public readonly EOGUIContextWriterAction updater;

            public SetValue(GUIContent label)
            {
                updater = (IEOGUIElement element, EOGUIContextStatus status) =>
                {
                    status.label = label;
                };
            }

            public EOGUIContextWriterAction Updater => updater;
        }

        public readonly struct SetText : IEOGUIContextLabel
        {
            public readonly EOGUIContextWriterAction updater;

            public SetText(string text)
            {
                var label = new GUIContent(text);
                updater = (IEOGUIElement element, EOGUIContextStatus status) =>
                {
                    status.label = label;
                };
            }

            public EOGUIContextWriterAction Updater => updater;
        }
    }
}

