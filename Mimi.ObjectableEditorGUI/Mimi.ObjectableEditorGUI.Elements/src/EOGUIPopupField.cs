using System;
using System.Collections.Generic;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIPopupField<T> : EOGUIPopupField<EOGUIPopupField<T>, T>
    {
        public EOGUIPopupField(T defaultValue) : base(defaultValue)
        {
        }
    }

    public abstract class EOGUIPopupField<TSelf, T> : EOGUIElement<TSelf>, IEOGUIElementField<TSelf, T>
        where TSelf : EOGUIPopupField<TSelf, T>
    {
        private T value;

        public override EOGUIHeightType HeightType => EOGUIHeightType.Static;
        public EOGUIPopup<T> Popup { get; }
        public T Value
        {
            get => value;
            set
            {
                if (!Comparer.Equals(this.value, value))
                {
                    this.value = value;
                    OnChangedValue?.Invoke(value);
                }
            }
        }
        public EqualityComparer<T> Comparer { get; set; } = EqualityComparer<T>.Default;

        protected EOGUIPopupField(T defaultValue)
        {
            Popup = new EOGUIPopup<T>();
            Value = defaultValue;
        }

        public event Action<T> OnChangedValue;

        public override float GetElementHeight()
        {
            return Popup.GetLineHeight(this);
        }

        public override void OnElementGUI()
        {
            Value = Popup.OnLineGUI(this, value);
            EOGUIFieldClipBoard.ContextMenu(this);
        }
    }
}