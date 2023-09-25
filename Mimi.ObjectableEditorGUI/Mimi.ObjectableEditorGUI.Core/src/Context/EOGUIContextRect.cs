using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Context
{
    public static class EOGUIContextRect
    {
        public static readonly SetRootRect RootRect;
        public static SetValue Value(in Rect rect) => new SetValue(rect);
        public static SetOffset Offset(RectOffset offset) => new SetOffset(offset);
        public static SetOffset Offset(in (RectOffset offset, bool add) arg) => new SetOffset(arg);

        static EOGUIContextRect()
        {
            RootRect = new SetRootRect();
        }

        public readonly struct SetRootRect : IEOGUIContextRect
        {
            private static readonly EOGUIContextWriterAction updater;

            static SetRootRect()
            {
                updater = (IEOGUIElement element, EOGUIContextStatus status) =>
                {
                    var rect = element.Context.RootRect;
                    status.rect = element.Context.RootRect;
                };
            }

            public EOGUIContextWriterAction Updater => updater;
        }

        public readonly struct SetValue : IEOGUIContextRect
        {
            public readonly EOGUIContextWriterAction updater;

            public SetValue(in Rect rect)
            {
                updater = GetUpdater(rect);
            }

            public static EOGUIContextWriterAction GetUpdater(Rect rect)
            {
                return (IEOGUIElement element, EOGUIContextStatus status) =>
                {
                    status.rect.Set(rect.x, rect.y, rect.width, rect.height);
                };
            }

            public EOGUIContextWriterAction Updater => updater;
        }

        public readonly struct SetOffset : IEOGUIContextRect
        {
            public readonly EOGUIContextWriterAction updater;

            public SetOffset(RectOffset offset)
            {
                updater = GetUpdater(offset);
            }

            public static EOGUIContextWriterAction GetUpdater(RectOffset offset)
            {
                return (IEOGUIElement element, EOGUIContextStatus status) =>
                {
                    status.rect = offset.Remove(status.rect);
                };
            }

            public SetOffset(in (RectOffset offset, bool add) arg)
            {
                if (arg.add)
                {
                    var offset = arg.offset;
                    updater = (IEOGUIElement element, EOGUIContextStatus status) =>
                    {
                        status.rect = offset.Add(status.rect);
                    };
                }
                else
                {
                    var offset = arg.offset;
                    updater = (IEOGUIElement element, EOGUIContextStatus status) =>
                    {
                        status.rect = offset.Remove(status.rect);
                    };
                }
            }

            public EOGUIContextWriterAction Updater => updater;
        }
    }
}