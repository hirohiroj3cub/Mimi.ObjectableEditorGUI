using UnityEngine;
using ITParam = Mimi.ObjectableEditorGUI.Context.IEOGUIContextParam;

namespace Mimi.ObjectableEditorGUI.Context
{
    public class EOGUIContextWriterAny : EOGUIContextWriter<ITParam>,
        IEOGUIContextWriterArgIn<ITParam, (object key, object value)>,
        IEOGUIContextWriterArg<ITParam, GUIContent>,
        IEOGUIContextWriterArgIn<ITParam, Rect>,
        IEOGUIContextWriterArg<ITParam, RectOffset>,
        IEOGUIContextWriterArgIn<ITParam, (RectOffset offset, bool add)>
    {
        public void Add(in (object key, object value) arg, string key = "")
        {
            Add(EOGUIContextCustomContext.KeyValue(arg), key);
        }

        public void Add(GUIContent arg, string key = "")
        {
            Add(EOGUIContextLabel.Value(arg), key);
        }

        public void Add(in Rect arg, string key = "")
        {
            Add(EOGUIContextRect.Value(arg), key);
        }

        public void Add(RectOffset arg, string key = "")
        {
            Add(EOGUIContextRect.Offset(arg), key);
        }

        public void Add(in (RectOffset offset, bool add) arg, string key = "")
        {
            Add(EOGUIContextRect.Offset(arg), key);
        }

        public void AddHead(in (object key, object value) arg, string key = "")
        {
            AddHead(EOGUIContextCustomContext.KeyValue(arg), key);
        }

        public void AddHead(GUIContent arg, string key = "")
        {
            AddHead(EOGUIContextLabel.Value(arg), key);
        }

        public void AddHead(in Rect arg, string key = "")
        {
            AddHead(EOGUIContextRect.Value(arg), key);
        }

        public void AddHead(RectOffset arg, string key = "")
        {
            AddHead(EOGUIContextRect.Offset(arg), key);
        }

        public void AddHead(in (RectOffset offset, bool add) arg, string key = "")
        {
            AddHead(EOGUIContextRect.Offset(arg), key);
        }
    }
}