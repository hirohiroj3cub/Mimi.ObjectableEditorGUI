using ITParam = Mimi.ObjectableEditorGUI.Context.IEOGUIContextCustomContext;
using TParam = Mimi.ObjectableEditorGUI.Context.EOGUIContextCustomContext;

namespace Mimi.ObjectableEditorGUI.Context
{
    public class EOGUIContextWriterCustomContext : EOGUIContextWriter<ITParam>,
        IEOGUIContextWriterArgIn<ITParam, (object key, object value)>
    {
        public void Add(in (object key, object value) arg, string key = "")
        {
            Add(TParam.KeyValue(arg), key);
        }

        public void AddHead(in (object key, object value) arg, string key = "")
        {
            AddHead(TParam.KeyValue(arg), key);
        }
    }
}