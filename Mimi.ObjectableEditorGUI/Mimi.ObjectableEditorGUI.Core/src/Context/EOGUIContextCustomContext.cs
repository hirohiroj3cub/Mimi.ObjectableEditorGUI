namespace Mimi.ObjectableEditorGUI.Context
{
    public static class EOGUIContextCustomContext
    {
        public static SetKeyValue KeyValue(in (object key, object value) arg) => new SetKeyValue(arg.key, arg.value);

        public readonly struct SetKeyValue : IEOGUIContextCustomContext
        {
            private readonly EOGUIContextWriterAction updater;

            public SetKeyValue(object key, object value)
            {
                updater = (IEOGUIElement element, EOGUIContextStatus status) =>
                {
                    status.CustomContext[key] = value;
                };
            }

            public EOGUIContextWriterAction Updater => updater;
        }
    }
}