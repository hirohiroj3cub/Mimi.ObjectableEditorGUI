namespace Mimi.ObjectableEditorGUI.Context
{
    public interface IEOGUIContextWriterArg<TParam, in TArg> : IEOGUIContextWriter<TParam>
        where TParam : IEOGUIContextParam
    {
        void Add(TArg arg, string key = "");
        void AddHead(TArg arg, string key = "");
    }

    public interface IEOGUIContextWriterArgIn<TParam, TArg> : IEOGUIContextWriter<TParam>
        where TParam : IEOGUIContextParam
        where TArg : struct
    {
        void Add(in TArg arg, string key = "");
        void AddHead(in TArg arg, string key = "");
    }
}