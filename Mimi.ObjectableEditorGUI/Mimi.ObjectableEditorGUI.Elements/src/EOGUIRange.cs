using System;
using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIRange<TField> : EOGUIRange<EOGUIRange<TField>, TField>
        where TField : EOGUIElement, IEOGUIElementField<TField, float>
    {
        public EOGUIRange(TField field, float minLimit, float maxLimit) : base(field, minLimit, maxLimit)
        {
        }
    }

    public abstract class EOGUIRange<TSelf, TField> : EOGUIReadOnlyList<TSelf, IEOGUIElementChild>, IEOGUIElementField<TSelf, float>
        where TSelf : EOGUIRange<TSelf, TField>
        where TField : EOGUIElement, IEOGUIElementField<TField, float>
    {
        public sealed class Slider : EOGUIElement<Slider>, IEOGUIElementField<Slider, float>
        {
            public Slider(TField field, float minLimit, float maxLimit)
            {
                MinLimit = minLimit;
                MaxLimit = maxLimit;
                Field = field;
                field.OnChangedValue += OnChangedValue;
            }

            public override EOGUIHeightType HeightType => EOGUIHeightType.Static;
            public TField Field { get; }
            public float MinLimit { get; set; }
            public float MaxLimit { get; set; }
            public float Delta { get; private set; }
            public float Value { get => Field.Value; set => Field.Value = Clamp(value); }

            public event Action<float>? OnChangedValue;

            public override float GetElementHeight()
            {
                return EditorGUIUtility.singleLineHeight;
            }

            public override void OnElementGUI()
            {
                float oldValue = Field.Value;
                float newValue = EditorGUI.Slider(Status.rect, ValidLabel, Clamp(oldValue), MinLimit, MaxLimit);
                Delta = newValue - oldValue;
                Field.Value = newValue;

                EOGUIFieldClipBoard.ContextMenu(this);
            }

            public float Clamp(float value)
            {
                return Mathf.Clamp(value, MinLimit, MaxLimit);
            }
        }

        protected override EOGUIHeightType HeightType_Protected { get; set; }
        public TField Field { get; }
        public Slider RangeSlider { get; }
        public float Value { get => Field.Value; set => Field.Value = RangeSlider.Clamp(value); }

        public EOGUIRange(TField field, float minLimit, float maxLimit) :
            base(InitElements(field, minLimit, maxLimit, out var rangeSlider), true)
        {
            Field = field;
            RangeSlider = rangeSlider;
            rangeSlider.OnChangedValue += OnChangedValue;
        }

        public event Action<float>? OnChangedValue;

        private static IEOGUIElementChild[] InitElements(TField field, float minLimit, float maxLimit, out Slider rangeSlider)
        {
            field.Options.Visible = false;

            var label = new EOGUILabelField()
            {
                Options =
                {
                    WidthFix = 110,
                    WidthRatio = 0f
                }
            };

            var rangeSliderLabel = GUIContent.none;
            rangeSlider = new Slider(field, minLimit, maxLimit)
            {
                Options =
                {
                    WidthFix = 110,
                    WidthRatio = 2,
                    CustomContextUpdate = (element, contextUpdate) =>
                    {
                        element.Status.label = rangeSliderLabel;
                    }
                }
            };

            return new IEOGUIElementChild[] { field, label, rangeSlider };
        }

        public override void OnElementGUI()
        {
            base.OnElementGUI();

            EOGUIFieldClipBoard.ContextMenu(this);
        }
    }
}