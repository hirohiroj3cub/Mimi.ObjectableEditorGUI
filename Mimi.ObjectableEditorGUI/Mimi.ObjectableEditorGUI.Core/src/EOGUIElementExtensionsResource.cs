using Mimi.InspectorAnimation;
using Mimi.ObjectableEditorGUI.Context;

namespace Mimi.ObjectableEditorGUI
{
    public class EOGUIElementExtensionsResource
    {
        public EOGUIContext context;
        public readonly EOGUIContextStatusCache cachedStatus = new EOGUIContextStatusCache();
        public readonly InspectorAnimator animator = new InspectorAnimator(10.0, false);
        public readonly EOGUIElementHeights cachedHeight = new EOGUIElementHeights();
        public float cachedWidth;
        //public float labelWidth;
    }
}