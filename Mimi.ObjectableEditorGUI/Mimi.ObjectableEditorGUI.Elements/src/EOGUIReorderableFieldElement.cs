using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIReorderableFieldElement<T> : EOGUIReorderableFieldElement<EOGUIReorderableFieldElement<T>, T>
    {
        public EOGUIReorderableFieldElement(ReorderableList reorderableList, T initValue) : base(reorderableList, initValue)
        {
        }
    }

    public abstract class EOGUIReorderableFieldElement<TSelf, T> : EOGUIField<TSelf, T>
        where TSelf : EOGUIReorderableFieldElement<TSelf, T>
    {
        private static readonly GUIContent contextMenu_delete_label = new GUIContent("Delete");
        private static readonly GUIContent contextMenu_duplicate_label = new GUIContent("Duplicate");
        private readonly GenericMenu.MenuFunction contextMenu_delete_func;
        private readonly GenericMenu.MenuFunction contextMenu_duplicat_func;

        public ReorderableList ReorderableList { get; }
        protected EOGUIReorderableFieldElement(ReorderableList reorderableList, T initValue) : base(initValue)
        {
            ReorderableList = reorderableList;
            contextMenu_delete_func = () => ReorderableList.onRemoveCallback(ReorderableList);
            contextMenu_duplicat_func = () => ReorderableList.onAddCallback(ReorderableList);
        }

        public override void OnElementContextUpdate()
        {
            base.OnElementContextUpdate();
        }

        public override void OnElementGUI()
        {
            var indexRect = new Rect(Status.rect.x, Status.rect.y - 2, 40, EditorGUIUtility.singleLineHeight);
            GUI.Label(indexRect, $"[{Index}]");

            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 1 && Status.rect.Contains(e.mousePosition))
            {
                var contextMenu = new GenericMenu();
                contextMenu.AddItem(contextMenu_delete_label, false, contextMenu_delete_func);
                contextMenu.AddItem(contextMenu_duplicate_label, false, contextMenu_duplicat_func);
                contextMenu.ShowAsContext();
            }

            Status.rect.xMin += indexRect.width;
            EOGUIReorderableUtility.LabelTextVaild(ValidLabel, Index, Value);
            base.OnElementGUI();
        }
    }
}