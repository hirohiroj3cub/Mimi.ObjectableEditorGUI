using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Elements
{

    public sealed class EOGUISplitLine : EOGUIElement<EOGUISplitLine>
    {
        public EOGUISplitLine()
        {
            Options = new EOGUIElementOptions<EOGUISplitLine>()
            {
                Margin = new RectOffset(2, 2, 2, 2),
                Padding = new RectOffset(2, 2, 2, 2)
            };
        }

        public override EOGUIHeightType HeightType => EOGUIHeightType.Static;

        public override float GetElementHeight()
        {
            return 3;
        }

        public static Color topColor = new Color(1, 1, 1, 0.25f);
        public static Color bottomColor = new Color(0, 0, 0, 0.25f);

        public override void OnElementGUI()
        {
            if (Event.current.type == EventType.Repaint)
            {
                var boxRect = new Rect(Status.rect.x, Status.rect.yMax - 3, Status.rect.width, 1.5f);
                var tex = EditorGUIUtility.whiteTexture;
                Color oldColor = GUI.color;
                GUI.color = topColor;
                GUI.DrawTexture(boxRect, tex);
                boxRect.y += 1;
                GUI.color = bottomColor;
                GUI.DrawTexture(boxRect, tex);
                GUI.color = oldColor;
            }
        }
    }
}