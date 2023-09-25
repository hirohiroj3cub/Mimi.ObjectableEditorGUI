namespace Mimi.ObjectableEditorGUI.Context
{
    public interface IEOGUIContextParam
    {
        EOGUIContextWriterAction Updater { get; }
    }

    public interface IEOGUIContextCustomContext : IEOGUIContextParam { }
    public interface IEOGUIContextLabel : IEOGUIContextParam { }
    public interface IEOGUIContextRect : IEOGUIContextParam { }
    public interface IEOGUIContextSerializedProperty : IEOGUIContextParam { }
}