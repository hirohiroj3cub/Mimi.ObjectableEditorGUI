using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIFoldout : EOGUIElementWraper<EOGUIFoldout, IEOGUIElementChild>
    {
        private bool isExpanded;

        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (isExpanded != value)
                {
                    isExpanded = value;
                    Context?.Drawer.SetFlagOfHeightUpdateAll();
                }
            }
        }

        public EOGUIFoldout(IEOGUIElementChild element) : base(element)
        {
        }

        public override float GetElementHeight()
        {
            return isExpanded ? EditorGUIUtility.singleLineHeight + base.GetElementHeight() : EditorGUIUtility.singleLineHeight;
        }

        public override void OnElementGUI()
        {
            var foldOutRect = Status.rect;

            foldOutRect.height = EditorGUIUtility.singleLineHeight;

            var e = Event.current;
            bool clicked = e.type == EventType.MouseDown && e.button == 0 && foldOutRect.Contains(e.mousePosition);
            if (clicked) IsExpanded = !isExpanded;
            EditorGUI.Foldout(foldOutRect, isExpanded, ValidLabel);

            if (isExpanded)
            {
                Status.rect.yMin += EditorGUIUtility.singleLineHeight;
                //EditorGUI.indentLevel++;
                base.OnElementGUI();
                //EditorGUI.indentLevel--;
            }
        }
    }
}