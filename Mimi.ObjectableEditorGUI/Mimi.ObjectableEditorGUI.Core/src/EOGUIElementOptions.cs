using UnityEngine;

namespace Mimi.ObjectableEditorGUI
{
    public abstract class EOGUIElementOptions
    {
        public EOGUIElementOptions()
        {
            Visible = true;
            Margin = new RectOffset(0, 0, 0, 0);
            Padding = new RectOffset(0, 0, 0, 0);
            WidthFix = 0f;
            WidthRatio = 1f;
            Align = 0f;
            Disabled = false;
            BackgroundStyle = null;
            ContentColor = null;
            BackgroundColor = null;
            UsePropertyScope = false;
        }

        public EOGUIElementOptions(EOGUIElementOptions options)
        {
            Visible = options.Visible;
            Margin = options.Margin;
            Padding = options.Padding;
            WidthFix = options.WidthFix;
            WidthRatio = options.WidthRatio;
            Align = options.Align;
            Disabled = options.Disabled;
            BackgroundStyle = options.BackgroundStyle;
            ContentColor = options.ContentColor;
            BackgroundColor = options.BackgroundColor;
            UsePropertyScope = options.UsePropertyScope;
        }

        public bool Visible { get; set; }
        public RectOffset Margin { get; set; }
        public RectOffset Padding { get; set; }
        public float WidthFix { get; set; }
        public float WidthRatio { get; set; }
        public float Align { get; set; }
        public bool? Disabled { get; set; }
        public GUIStyle BackgroundStyle { get; set; }
        public Color? ContentColor { get; set; }
        public Color? BackgroundColor { get; set; }
        public bool UsePropertyScope { get; set; }

        public EOGUIElementInvokers.ContextUpdate.EventHandler<IEOGUIElement> CustomContextUpdate { get; set; }
        public EOGUIElementInvokers.PreUpdate.EventHandler<IEOGUIElement> CustomPreUpdate { get; set; }
        public EOGUIElementInvokers.GUI.EventHandler<IEOGUIElement> CustomGUI { get; set; }

        public abstract void CallCustomContextUpdate(IEOGUIElement element);

        public abstract void CallCustomPreUpdate(IEOGUIElement element);

        public abstract void CallCustomGUI(IEOGUIElement element);

        public float GetAlignSize(float maxSize, float size)
        {
            var range = maxSize - size;
            return range * (Align + 1f) * 0.5f;
        }
    }

    public class EOGUIElementOptions<TSelf> : EOGUIElementOptions
        where TSelf : class, IEOGUIElement
    {
        private TSelf elementT;
        private readonly EOGUIElementInvokers.ContextUpdate contextUpdateInvoker = new EOGUIElementInvokers.ContextUpdate();
        private readonly EOGUIElementInvokers.PreUpdate preUpdateInvoker = new EOGUIElementInvokers.PreUpdate();
        private readonly EOGUIElementInvokers.GUI guiInvoker = new EOGUIElementInvokers.GUI();

        public EOGUIElementOptions(EOGUIElementOptions options) : base(options)
        {
        }

        public EOGUIElementOptions() : base()
        {
        }

        public EOGUIElementInvokers.ContextUpdate.EventHandler<TSelf, IEOGUIElement> CustomContextUpdateT { set; get; }
        public EOGUIElementInvokers.PreUpdate.EventHandler<TSelf, IEOGUIElement> CustomPreUpdateT { set; get; }
        public EOGUIElementInvokers.GUI.EventHandler<TSelf, IEOGUIElement> CustomGUIT { set; get; }

        public override void CallCustomContextUpdate(IEOGUIElement element)
        {
            UpdateElementT(element);

            using var scope = contextUpdateInvoker.Init(element);
            contextUpdateInvoker.Invoke(elementT, element, CustomContextUpdateT, CustomContextUpdate);
        }

        public override void CallCustomPreUpdate(IEOGUIElement element)
        {
            UpdateElementT(element);

            using var scope = preUpdateInvoker.Init(element);
            preUpdateInvoker.Invoke(elementT, element, CustomPreUpdateT, CustomPreUpdate);
        }

        public override void CallCustomGUI(IEOGUIElement element)
        {
            UpdateElementT(element);

            using var scope = guiInvoker.Init(element);
            guiInvoker.Invoke(elementT, element, CustomGUIT, CustomGUI);
        }

        private void UpdateElementT(IEOGUIElement element)
        {
            if (elementT == null || !elementT.Equals(element)) elementT = (TSelf)element;
        }
    }
}