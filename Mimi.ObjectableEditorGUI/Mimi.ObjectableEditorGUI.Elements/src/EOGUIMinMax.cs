using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIMinMax<TField> : EOGUIMinMax<EOGUIMinMax<TField>, TField>
        where TField : EOGUIElement, IEOGUIElementField<TField, float>
    {
        public EOGUIMinMax(TField min, TField max, float minLimit, float maxLimit) : base(min, max, minLimit, maxLimit)
        {
        }
    }

    public abstract class EOGUIMinMax<TSelf, TField> : EOGUIReadOnlyList<TSelf, IEOGUIElementChild>
        where TSelf : EOGUIMinMax<TSelf, TField>
        where TField : class, IEOGUIElementChild, IEOGUIElementField<TField, float>
    {
        public sealed class Slider : EOGUIElement<Slider>
        {
            private float oldSliderMin;
            private float oldSliderMax;

            public TField Min { get; }
            public TField Max { get; }
            public float MinLimit { get; set; }
            public float MaxLimit { get; set; }
            public float MinDelta { get; private set; }
            public float MaxDelta { get; private set; }
            public override EOGUIHeightType HeightType => EOGUIHeightType.Static;

            public Slider(TField min, TField max, float minLimit, float maxLimit)
            {
                Min = min;
                Max = max;
                MinLimit = minLimit;
                MaxLimit = maxLimit;
            }

            public override float GetElementHeight()
            {
                return EditorGUIUtility.singleLineHeight;
            }

            public override void OnElementGUI()
            {
                var oldMin = Min.Value;
                var oldMax = Max.Value;

                var sliderMin = oldMin;
                var sliderMax = oldMax;

                EditorGUI.MinMaxSlider(Status.rect, ValidLabel, ref sliderMin, ref sliderMax, MinLimit, MaxLimit);

                if (sliderMin > sliderMax)
                {
                    if (oldSliderMax == sliderMax)
                    {
                        sliderMax = sliderMin = Clamp(sliderMax);
                    }
                    else if (oldSliderMin == sliderMin)
                    {
                        sliderMax = sliderMin = Clamp(sliderMin);
                    }
                    else
                    {
                        sliderMax = sliderMin = Clamp((sliderMax + sliderMin) * 0.5f);
                    }
                }
                else
                {
                    (sliderMin, sliderMax) = (ClampMin(sliderMin, sliderMax), ClampMax(sliderMax, sliderMin));
                }

                MinDelta = sliderMin - oldMin;
                MaxDelta = sliderMax - oldMax;

                if (sliderMin != oldMin) Min.Value = sliderMin;
                if (sliderMax != oldMax) Max.Value = sliderMax;

                oldSliderMin = sliderMin;
                oldSliderMax = sliderMax;
            }

            public float Clamp(float value)
            {
                return Mathf.Clamp(value, MinLimit, MaxLimit);
            }

            public float ClampMin(float value, float maxValue)
            {
                return Mathf.Clamp(value, MinLimit, maxValue);
            }

            public float ClampMax(float value, float minValue)
            {
                return Mathf.Clamp(value, minValue, MaxLimit);
            }
        }

        public EOGUIMinMax(TField minField, TField maxField, float minLimit, float maxLimit) :
            base(InitElements(minField, maxField, minLimit, maxLimit, out var minMaxSlider), false)
        {
            MinField = minField;
            MaxField = maxField;
            MinMaxSlider = minMaxSlider;
        }

        private static IEOGUIElementChild[] InitElements(TField minField, TField maxField,
            float minLimit, float maxLimit, out Slider minMaxSlider)
        {
            var label = new EOGUILabelField()
            {
                Options =
                {
                    WidthFix = 110,
                    WidthRatio = 0f
                }
            };

            var minLabel = new GUIContent("Min");
            minField.Options.WidthFix = 40;
            minField.Options.WidthRatio = 1f;
            minField.Options.CustomContextUpdate = minField.Options.CustomContextUpdate.Chain((element, contextUpdate, old) =>
            {
                old(element, contextUpdate);
                element.Status.label = minLabel;
            });

            var maxLabel = new GUIContent("Max");
            maxField.Options.WidthFix = 40;
            maxField.Options.WidthRatio = 1f;

            maxField.Options.CustomContextUpdate = maxField.Options.CustomContextUpdate.Chain((element, contextUpdate, old) =>
            {
                old(element, contextUpdate);
                element.Status.label = maxLabel;
            });

            var minMaxSliderLabel = GUIContent.none;
            minMaxSlider = new Slider(minField, maxField, minLimit, maxLimit)
            {
                Options =
                {
                    CustomContextUpdate = (element, contextUpdate) =>
                    {
                        contextUpdate.Invoke();
                        element.Status.label = minMaxSliderLabel;
                    }
                }
            };

            var minMax = new EOGUIReadOnlyList(new IEOGUIElementChild[] { label, minField, maxField }, true);
            return new IEOGUIElementChild[] { minMax, minMaxSlider };
        }

        protected override EOGUIHeightType HeightType_Protected { get; set; } = EOGUIHeightType.Static;

        public TField MinField { get; }
        public TField MaxField { get; }
        public Slider MinMaxSlider { get; }

        public float MinValue
        {
            get => MinField.Value;
            set
            {
                MinField.Value = MinMaxSlider.ClampMin(value, MaxField.Value);
            }
        }
        public float MaxValue
        {
            get => MaxField.Value;
            set
            {
                MaxField.Value = MinMaxSlider.ClampMax(value, MinField.Value);
            }
        }
    }
}