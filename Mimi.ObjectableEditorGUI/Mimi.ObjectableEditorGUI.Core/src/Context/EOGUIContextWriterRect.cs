using UnityEngine;
using ITParam = Mimi.ObjectableEditorGUI.Context.IEOGUIContextRect;
using TParam = Mimi.ObjectableEditorGUI.Context.EOGUIContextRect;

namespace Mimi.ObjectableEditorGUI.Context
{
    public class EOGUIContextWriterRect : EOGUIContextWriter<ITParam>,
        IEOGUIContextWriterArgIn<ITParam, Rect>,
        IEOGUIContextWriterArg<ITParam, RectOffset>,
        IEOGUIContextWriterArgIn<ITParam, (RectOffset offset, bool add)>
    {
        public void Add(in Rect arg, string key = "")
        {
            Add(TParam.Value(arg), key);
        }

        public void Add(RectOffset arg, string key = "")
        {
            Add(TParam.Offset(arg), key);
        }

        public void Add(in (RectOffset offset, bool add) arg, string key = "")
        {
            Add(TParam.Offset(arg), key);
        }

        public void AddHead(in Rect arg, string key = "")
        {
            AddHead(TParam.Value(arg), key);
        }

        public void AddHead(RectOffset arg, string key = "")
        {
            AddHead(TParam.Offset(arg), key);
        }

        public void AddHead(in (RectOffset offset, bool add) arg, string key = "")
        {
            AddHead(TParam.Offset(arg), key);
        }
    }
}