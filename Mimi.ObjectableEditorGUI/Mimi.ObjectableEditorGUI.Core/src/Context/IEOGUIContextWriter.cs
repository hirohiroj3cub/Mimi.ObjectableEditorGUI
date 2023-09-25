namespace Mimi.ObjectableEditorGUI.Context
{
    public interface IEOGUIContextWriter : IEOGUIContextWriterReadonly
    {
        bool Remove(string key = "");
        bool RemoveAll(string[] keys);
        void Clear();
    }

    public interface IEOGUIContextWriter<TParam> : IEOGUIContextWriter, IEOGUIContextWriterReadonly<TParam>
        where TParam : IEOGUIContextParam
    {
        void Add<T>(T param, string key = "") where T : struct, TParam;
        void Add<T>(IEOGUIContextWriterReadonly<T> other, string key = "") where T : TParam;
        void AddHead<T>(T param, string key = "") where T : struct, TParam;
        void AddHead<T>(IEOGUIContextWriterReadonly<T> other, string key = "") where T : TParam;
    }
}