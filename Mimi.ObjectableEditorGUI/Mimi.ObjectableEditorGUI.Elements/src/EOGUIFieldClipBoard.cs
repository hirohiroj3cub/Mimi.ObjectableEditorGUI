using System;
using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public static class EOGUIFieldClipBoard
    {
        [Serializable]
        private class ClipData<T>
        {
            [SerializeField]
            public T value;
            [SerializeField]
            public string typeName;

            public ClipData(T value)
            {
                this.value = value;
                typeName = typeof(T).FullName;
            }
        }

        private static class ClipDatas<T>
        {
            public static ClipData<T>? clipData;
        }

        private static readonly GUIContent label_copy = new GUIContent("Copy Field");
        private static readonly GUIContent label_paste = new GUIContent("Paste Field");

        public static void ContextMenu<T>(IEOGUIElementField<T> field)
        {
            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 1 && field.Status.rect.Contains(e.mousePosition))
            {
                var contextMenu = new GenericMenu();


                contextMenu.AddItem(label_copy, false, () =>
                {
                    ref var clipData = ref ClipDatas<T>.clipData;

                    if (clipData == null)
                    {
                        clipData = new ClipData<T>(field.Value);
                    }
                    else
                    {
                        clipData.value = field.Value;
                    }

                    UnityEngine.GUIUtility.systemCopyBuffer = JsonUtility.ToJson(clipData);
                });

                try
                {
                    ref var clipData = ref ClipDatas<T>.clipData;
                    JsonUtility.FromJsonOverwrite(UnityEngine.GUIUtility.systemCopyBuffer, clipData);
                    if (clipData?.typeName == typeof(T).FullName)
                    {
                        var value = clipData.value;
                        contextMenu.AddItem(label_paste, false, () => field.Value = value);
                    }
                    else
                    {
                        contextMenu.AddDisabledItem(label_paste);
                    }
                }
                catch (Exception)
                {
                    contextMenu.AddDisabledItem(label_paste);
                }

                contextMenu.ShowAsContext();
            }
        }
    }
}