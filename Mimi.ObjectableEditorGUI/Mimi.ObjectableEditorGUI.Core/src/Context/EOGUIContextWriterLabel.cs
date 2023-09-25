using UnityEngine;
using ITParam = Mimi.ObjectableEditorGUI.Context.IEOGUIContextLabel;
using TParam = Mimi.ObjectableEditorGUI.Context.EOGUIContextLabel;

namespace Mimi.ObjectableEditorGUI.Context
{
    public class EOGUIContextWriterLabel : EOGUIContextWriter<ITParam>,
        IEOGUIContextWriterArg<ITParam, GUIContent>,
        IEOGUIContextWriterArg<ITParam, string>
    {
        public void Add(string arg, string key = "")
        {
            Add(TParam.Text(arg), key);
        }

        public void Add(GUIContent arg, string key = "")
        {
            Add(TParam.Value(arg), key);
        }

        public void AddHead(string arg, string key = "")
        {
            AddHead(TParam.Text(arg), key);
        }

        public void AddHead(GUIContent arg, string key = "")
        {
            AddHead(TParam.Value(arg), key);
        }
    }
}