namespace Mimi.ObjectableEditorGUI.Context
{
    public static class EOGUIContextSerializedProperty
    {
        public static readonly SetRootProperty RootProperty;
        public static SetRelative Relative(string relativePath) => new SetRelative(relativePath);
        public static SetIndex Index(int index) => new SetIndex(index);

        static EOGUIContextSerializedProperty()
        {
            RootProperty = new SetRootProperty();
        }

        public readonly struct SetRootProperty : IEOGUIContextSerializedProperty
        {
            private static readonly EOGUIContextWriterAction updater;

            static SetRootProperty()
            {
                updater = (IEOGUIElement element, EOGUIContextStatus status) =>
                {
                    status.SerializedProperty = element.Context.RootProperty;
                };
            }

            public EOGUIContextWriterAction Updater => updater;
        }

        public readonly struct SetRelative : IEOGUIContextSerializedProperty
        {
            public readonly EOGUIContextWriterAction updater;

            public SetRelative(string path)
            {
                updater = (IEOGUIElement element, EOGUIContextStatus status) =>
                {
                    status.SerializedProperty = status.SerializedProperty.FindPropertyRelative(path);
                };
            }

            public EOGUIContextWriterAction Updater => updater;
        }

        public readonly struct SetIndex : IEOGUIContextSerializedProperty
        {
            public readonly EOGUIContextWriterAction updater;

            public SetIndex(int index)
            {
                updater = (IEOGUIElement element, EOGUIContextStatus status) =>
                {
                    status.SerializedProperty = status.SerializedProperty.GetArrayElementAtIndex(index);
                };
            }

            public EOGUIContextWriterAction Updater => updater;
        }
    }
}