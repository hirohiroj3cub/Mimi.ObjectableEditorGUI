using Mimi.ObjectableEditorGUI.Context;
using Mimi.ObjectableEditorGUI.Elements;
using Mimi.ObjectableEditorGUI.GUIUtility;
using Mimi.InspectorAnimation;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mimi.ObjectableEditorGUI.Sample
{
    public class SampleDataAEqualityComparer : EqualityComparer<SampleDataA>
    {
        public static SampleDataAEqualityComparer Instance { get; } = new();

        private SampleDataAEqualityComparer()
        {
        }

        public override bool Equals(SampleDataA x, SampleDataA y)
        {
            return x.intValue == y.intValue &&
                   x.stringValue.Equals(y.stringValue, StringComparison.Ordinal) &&
                   x.minValue == y.minValue &&
                   x.maxValue == y.maxValue &&
                   x.rangeValue == y.rangeValue;
        }

        public override int GetHashCode(SampleDataA obj)
        {
            throw new NotImplementedException();
        }
    }

    [CustomPropertyDrawer(typeof(SampleDataA))]
    public class SampleDataADrawer : EOGUIPropertyDrawer
    {
        private readonly InspectorAnimator animator = new(TimeSpan.FromSeconds(10), false);

        public override IEOGUIElementParent InitialRootElement()
        {
            SerializedProperty intValue = null;
            SerializedProperty stringValue = null;
            SerializedProperty minValue = null;
            SerializedProperty maxValue = null;
            SerializedProperty rangeValue = null;
            SampleDataA sampleDataA = new();

            SampleDataA Getter(SerializedProperty property)
            {
                if (intValue == null)
                {
                    intValue = property.FindPropertyRelative(nameof(intValue));
                    stringValue = property.FindPropertyRelative(nameof(stringValue));
                    minValue = property.FindPropertyRelative(nameof(minValue));
                    maxValue = property.FindPropertyRelative(nameof(maxValue));
                    rangeValue = property.FindPropertyRelative(nameof(rangeValue));
                }

                sampleDataA.intValue = intValue.intValue;
                sampleDataA.stringValue = stringValue.stringValue;
                sampleDataA.minValue = minValue.floatValue;
                sampleDataA.maxValue = maxValue.floatValue;
                sampleDataA.rangeValue = rangeValue.floatValue;

                return sampleDataA;
            }

            void Setter(SerializedProperty property, SampleDataA value)
            {
                sampleDataA = value;
                if (intValue == null)
                {
                    intValue = property.FindPropertyRelative(nameof(intValue));
                    stringValue = property.FindPropertyRelative(nameof(stringValue));
                    minValue = property.FindPropertyRelative(nameof(minValue));
                    maxValue = property.FindPropertyRelative(nameof(maxValue));
                    rangeValue = property.FindPropertyRelative(nameof(rangeValue));
                }

                intValue.intValue = value.intValue;
                stringValue.stringValue = value.stringValue;
                minValue.floatValue = value.minValue;
                maxValue.floatValue = value.maxValue;
                rangeValue.floatValue = value.rangeValue;
            }

            var animationElementLabel = new GUIContent();
            var animationElement = new EOGUICustom()
            {
                Options =
                {
                    WidthFix = 150,
                    WidthRatio = 0,
                    Align = 0,
                },
                OnElementContextUpdateCustom = (EOGUICustom element) =>
                {
                    var status = element.Status;
                    element.Status.label = animationElementLabel;
                },
                OnElementPreUpdateCustom = (EOGUICustom element) =>
                {
                    var t = Mathf.Deg2Rad * (float)animator.TotalAnimateFrameTime.TotalSeconds * 90;
                    element.Options.Align = Mathf.Sin(t);
                },
                GetElementHeightCustom = (EOGUICustom element) =>
                {
                    return 25f;
                },
                OnElementGUICustom = (EOGUICustom element) =>
                {
                    using var fontSetting = new GUIFontSettingScope(new(GUI.skin.button, fontSize: 10));

                    animator.OnGUIUpdate(element.Status.SerializedProperty);

                    if (animator.IsPlaying)
                    {
                        animationElementLabel.text = $"Animation !! ({animator.TotalAnimateFrameTime:m\\:ss\\:f})";
                    }
                    else
                    {
                        animationElementLabel.text = "Start Animation";
                    }

                    if (GUI.Button(element.Status.rect, element.Status.ValidLabel, GUI.skin.button))
                    {
                        if (animator.IsPlaying)
                        {
                            animator.Stop();
                        }
                        else
                        {
                            animator.StartRepeat();
                        }
                    }
                },
            };

            var timer = new EOGUILabelField("Last Update Time", "time")
            {
                Options =
                {
                    CustomGUIT = (element, gui, chain) =>
                    {
                        element.SubLabel.text = DateTime.Now.ToString("HH:mm:ss:ff");
                        chain.Invoke(element, gui);
                    },
                }
            };

            var property = new EOGUIPropertyField<SampleDataA>()
            {
                Options = { Visible = false },
                Getter = Getter,
                Setter = Setter,
            };

            var toString = new EOGUILabelField("ToString", "value")
            {
                Options =
                {
                    CustomGUIT = (element, gui, chain) =>
                    {
                        element.SubLabel.text =  property?.Value?.ToString();
                        chain.Invoke(element, gui);
                    },
                }
            };

            var intValueRandomButton = new EOGUIButton("Random intValue")
            {
                (EOGUIButton element) =>
                {
                    sampleDataA.intValue = Random.Range(0, 100);
                    property.Value = sampleDataA;
                }
            };

            var stringPopupLabel = new GUIContent("String Popup");
            var stringPopup = new EOGUIPopupPropertyField<string>(new() { "stringValue" })
            {
                Options =
                {
                    CustomContextUpdate = (element, contextUpdate) =>
                    {
                        contextUpdate.Invoke();
                        element.Status.label = stringPopupLabel;
                    }
                },
                Popup =
                {
                    ["Œ¢"] = "Dog",
                    ["”L"] = "Cat",
                    ["ŒÏ"] = "Fox",
                },
            };

            var minMaxValueFieldLabel = new GUIContent("Min Max Values");
            var minMaxValueField = new EOGUIMinMaxPropertyField(new() { "minValue" }, new() { "maxValue" }, -10, 10)
            {
                Options =
                {
                    CustomContextUpdate = (element, contextUpdate) =>
                    {
                        contextUpdate.Invoke();
                        element.Status.label = minMaxValueFieldLabel;
                    }
                },
            };

            var rangeValueFieldLabel = new GUIContent("Range Values");
            var rangeValueField = new EOGUIRangePropertyField(new() { "rangeValue" }, -10, -10)
            {
                Options =
                {
                    CustomContextUpdate = (element, contextUpdate) =>
                    {
                        contextUpdate.Invoke();
                        element.Status.label = rangeValueFieldLabel;
                    },
                    CustomGUIT = (element, gui, chain) =>
                    {
                        element.RangeSlider.MinLimit = minMaxValueField.MinValue;
                        element.RangeSlider.MaxLimit = minMaxValueField.MaxValue;
                        chain.Invoke(element, gui);
                    },
                }
            };

            var minMaxLabel = new GUIContent("Min Max Field");
            var minMax = new EOGUIMinMaxField(-5, 5, -10, 10)
            {
                Options =
                {
                    CustomContextUpdate = (element, contextUpdate) =>
                    {
                        contextUpdate.Invoke();
                        element.Status.label = minMaxLabel;
                    },
                }
            };

            var rangeLabel = new GUIContent("Range Field");
            var range = new EOGUIRangeField(0, -5, 5)
            {
                Options =
                {
                    CustomContextUpdate = (element, contextUpdate) =>
                    {
                        contextUpdate.Invoke();
                        element.Status.label = rangeLabel;
                    },
                    CustomGUIT = (element, gui, chain) =>
                    {
                        element.RangeSlider.MinLimit = minMax.MinValue;
                        element.RangeSlider.MaxLimit = minMax.MaxValue;
                    },
                }
            };

            var labelContainer = new EOGUIList(false)
            {
                Elements =
                {
                    new EOGUILabelField("This is Container")
                    {
                        Options =
                        {
                            CustomGUI = (element, gui) =>
                            {
                                using var fontScope = new GUIFontSettingScope(new(style: EditorStyles.label, fontStyle: FontStyle.BoldAndItalic));
                                gui.Invoke();
                            },
                        }
                    },
                    new EOGUISplitLine(),
                    new EOGUILabelField(),
                    new EOGUILabelField("LabelField 1"),
                    new EOGUILabelField("LabelField 2-1", "LabelField 2-2"),
                    new EOGUILabelField("parent name")
                    {
                        Options =
                        {
                            CustomContextUpdate = (element, customContext) =>
                            {
                                customContext.Invoke();
                                element.Status.label.text = customContext.Element.Parent?.Name ?? "null";
                            }
                        }
                    },
                }
            };

            var fieldContainer = new EOGUIList(false)
            {
            };

            var property2 = new EOGUIPropertyField<SampleDataA>()
            {
                Getter = Getter,
                Setter = Setter,
            };

            EOGUIButton GetFieldElementButton(string name, EOGUIContextWriterSerializedProperty selector)
            {
                return new EOGUIButton($"{name} Field")
                {
                    Options =
                    {
                        CustomGUI = (e, invoker) =>
                        {
                            e.Options.Disabled = fieldContainer.Count >= 5;
                        }
                    },
                    Action = (EOGUIButton gui) =>
                    {
                        var color = Color.HSVToRGB(Random.value, 1f, 1f);
                        var item = new EOGUIList(true)
                        {
                            Options =
                            {
                                BackgroundStyle = GUI.skin.box,
                                BackgroundColor = color,
                                ContentColor = color,
                                Padding = new RectOffset(10, 10, 10, 10)
                            },
                            Elements =
                            {
                                new EOGUIButton("Remove")
                                {
                                    Name = "Remove Button",
                                    Actions =
                                    {
                                        (EOGUIButton button) =>
                                        {
                                            fieldContainer.DelayRemove((list, e) => e == button.Parent);
                                        }
                                    },
                                    Options =
                                    {
                                        WidthFix = 90f,
                                        WidthRatio = 0f,
                                        Align = 0,
                                        CustomContextUpdate = (element, contextUpdate) =>
                                        {
                                            contextUpdate.Invoke();
                                            element.Status.label.text = $"[{element.Parent?.Index}] Remove";
                                        },
                                    }
                                },
                                new EOGUIProperty(selector)
                                {
                                    Options =
                                    {
                                        WidthFix = 120f,
                                        WidthRatio = 1f,
                                        Align = 0,
                                    }
                                },
                            }
                        };

                        fieldContainer.DelayAdd(list => item);
                    }
                };
            }

            var fieldButtons = new EOGUIList(true)
            {
                Options =
                {
                    CustomGUI = (element, gui) =>
                    {
                        using var font = new GUIFontSettingScope( new GUIFontSetting(GUI.skin.button, fontSize: 10));
                        gui.Invoke();
                    },
                },
                Elements =
                {
                    GetFieldElementButton("intValue", new() { "intValue" }),
                    GetFieldElementButton("stringValue", new() { "stringValue" }),
                    GetFieldElementButton("root", new() { EOGUIContextSerializedProperty.RootProperty }),
                    new EOGUIButton("Clear")
                    {
                        Options =
                        {
                            CustomGUI = (e, invoker) =>
                            {
                                e.Options.Disabled = fieldContainer.Count == 0;
                            }
                        },
                        Action = (e) =>
                        {
                            fieldContainer.Clear();
                        }
                    }
                }
            };

            var fieldArray = new EOGUIReorderableField<string>();
            var propertyFieldArray = new EOGUIReorderablePropertyField<int>(new() { "arrayValue" });
            var propertyArray = new EOGUIReorderableProperty(new() { "arrayDataC" });
            var switcerKey = new EOGUIPopupField<int>(0)
            {
                Popup =
                {
                    {"Field", 0},
                    {"PropertyField", 1},
                    {"Property", 2}
                }
            };
            var array = new EOGUISwitch<int>(0, fieldArray)
            {
                {1, propertyFieldArray},
                {2, propertyArray}
            };

            switcerKey.OnChangedValue += (v) =>
            {
                array.DelaySetKey(v);
            };

            var fieldArray2 = new EOGUIReorderableField<string>();
            var propertyFieldArray2 = new EOGUIReorderablePropertyField<int>(new() { "arrayValue" });
            var propertyArray2 = new EOGUIReorderableProperty(new() { "arrayDataC" });

            var pages = new EOGUIPages(new EOGUIPages<EOGUIPages>.Page[]
            {
                new("Field", fieldArray2, false),
                new("PropertyField", propertyFieldArray2, true),
                new("Property", propertyArray2, true),
            });

            //Total; 40.5KB?
            return new EOGUIList(false)
            {
                animationElement, //(+base)-- 19.0KB
                timer, //-- 19.5KB
                property, //-- 19.8KB
                new EOGUISplitLine(), //-- 20.2KB
                toString, //-- 0.6KB,
                intValueRandomButton, //-- 0.6KB
                stringPopup, //-- 0.9KB
                minMaxValueField, //-- 3.9KB (5 elements)
                rangeValueField, //-- 2.0KB (3 elements)
                new EOGUISplitLine(), //-- 0.4KB
                minMax, //-- 3.3KB (5 elements),
                range, //-- 2.1KB (3 elements)
                new EOGUIFoldout(labelContainer){Label = new("Label and Container")}, //-- 3.0KB
                fieldButtons, //-- 2.3KB
                fieldContainer, //-- 0.6KB
                property2, //--0.9KB
                switcerKey,
                new EOGUIFoldout(array),
                pages
            };
        }
    }
}