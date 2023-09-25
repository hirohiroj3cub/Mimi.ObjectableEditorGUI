using ITParam = Mimi.ObjectableEditorGUI.Context.IEOGUIContextSerializedProperty;
using TParam = Mimi.ObjectableEditorGUI.Context.EOGUIContextSerializedProperty;

namespace Mimi.ObjectableEditorGUI.Context
{
    public class EOGUIContextWriterSerializedProperty : EOGUIContextWriter<ITParam>,
        IEOGUIContextWriterArg<ITParam, string>,
        IEOGUIContextWriterArg<ITParam, int>
    {
        public void Add(string arg, string key = "")
        {
            Add(TParam.Relative(arg), key);
        }

        public void Add(int arg, string key = "")
        {
            Add(TParam.Index(arg), key);
        }

        public void AddHead(string arg, string key = "")
        {
            AddHead(TParam.Relative(arg), key);
        }

        public void AddHead(int arg, string key = "")
        {
            AddHead(TParam.Index(arg), key);
        }
    }
}