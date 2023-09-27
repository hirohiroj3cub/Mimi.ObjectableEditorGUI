using System;
using UnityEditor;
using UnityEngine;

namespace Mimi.InspectorAnimation.Sample
{
    [CustomPropertyDrawer(typeof(AnimationTest))]
    public class AnimationTestDrawer : PropertyDrawer
    {
        private readonly InspectorAnimator animation1;
        private static readonly InspectorAnimator animation2;

        static AnimationTestDrawer()
        {
            animation2 = new(0.5, false);
        }

        public AnimationTestDrawer()
        {
            animation1 = new(3.0, false);
            animation1.InitNewDrawer();
            animation2.InitNewDrawer();
            //Debug.Log("NewDrawer");
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 230;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            animation1.OnGUIUpdate(property);
            animation2.OnGUIUpdate(property);

            position = new RectOffset(2, 2, 2, 2).Remove(position);

            if (Event.current.type == EventType.Repaint)
            {
                GUI.skin.box.Draw(position, GUIContent.none, -1);
            }

            position = new RectOffset(2, 2, 2, 2).Remove(position);

            var size = new Vector2((position.width - 20) * 0.5f, 20);
            var button1Rect = new Rect()
            {
                size = size,
                center = position.center + new Vector2(-size.x * 0.5f, -95f)
            };

            var button2Rect = new Rect()
            {
                size = size,
                center = position.center + new Vector2(size.x * 0.5f, -95f)
            };

            var button3Rect = new Rect()
            {
                size = size,
                center = button1Rect.center + new Vector2(0, 20)
            };

            var button4Rect = new Rect()
            {
                size = size,
                center = button2Rect.center + new Vector2(0, 20)
            };

            var valueRect1 = new Rect()
            {
                size = new Vector2(position.width - 20, 20),
                center = position.center + new Vector2(0, -50)
            };

            var valueRect2 = new Rect()
            {
                size = new Vector2(position.width - 20, 20),
                center = valueRect1.center + new Vector2(0, 20)
            };

            var labelRect = new Rect()
            {
                size = new Vector2(position.width - 20, 20),
                center = valueRect2.center + new Vector2(0, 20)
            };

            AnimationButton(button1Rect, button2Rect, animation1);
            AnimationButton(button3Rect, button4Rect, animation2);
            AnimationValues(valueRect1, animation1);
            AnimationValues(valueRect2, animation2);

            GUI.Label(labelRect, $"def : {animation1.TotalAnimateFrameTime - animation2.TotalAnimateFrameTime}");

            var t1 = Math.PI * animation1.TotalAnimateFrameTime.TotalSeconds;
            var t2 = Math.PI * animation2.TotalAnimateFrameTime.TotalSeconds;

            var moveRect1 = new Rect()
            {
                size = new Vector2(100, 25),
                center = position.center + new Vector2(0, 50) +
                    new Vector2((position.width - 300) * 0.5f * (float)Math.Cos(t1),
                                25 * (float)Math.Sin(t1))
            };

            var moveRect2 = new Rect()
            {
                size = new Vector2(100, 25),
                center = position.center + new Vector2(0, 50) +
                    new Vector2((position.width - 100) * 0.5f * (float)Math.Cos(t2),
                                50 * (float)Math.Sin(t2))
            };

            if (GUI.Button(moveRect1, "Instance"))
            {
                animation1.TotalAnimateTime = TimeSpan.Zero;
            }
            if (GUI.Button(moveRect2, "Static"))
            {
                animation2.TotalAnimateTime = TimeSpan.Zero;
            }
        }

        private static void AnimationButton(Rect rect1, Rect rect2, InspectorAnimator animation)
        {
            string baseName = animation.IsStatic ? "[Static] " : "[Instance] ";
            if (animation.IsPlaying)
            {
                if (GUI.Button(rect1, baseName + "Cancel"))
                {
                    animation.Stop();
                }
            }
            else if (animation.IsCanceled)
            {
                if (GUI.Button(rect1, baseName + "Continue"))
                {
                    animation.Continue();
                }
            }
            else
            {
                if (GUI.Button(rect1, baseName + "Start"))
                {
                    animation.Start();
                }
            }

            if (animation.IsRepeating)
            {
                if (GUI.Button(rect2, baseName + "Stop Repeat"))
                {
                    animation.StopRepeat();
                }
            }
            else
            {
                if (GUI.Button(rect2, baseName + "Continu Repeat"))
                {
                    animation.ContinueRepeat();
                }
            }
        }

        private static void AnimationValues(Rect rect, InspectorAnimator animation)
        {
            string baseName = animation.IsStatic ? "[Static] " : "[Instance] ";
            var rect1 = rect; rect1.width = 100;
            var rect2 = rect; rect2.xMin += 102;
            var rect2_1 = rect2; rect2_1.xMax = rect2.center.x - 2;
            var rect2_2 = rect2; rect2_2.xMin = rect2.center.x + 2;

            EditorGUI.LabelField(rect1, baseName);

            var lw = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 50;

            var oldTime = animation.AnimateTime.TotalSeconds;
            var newTime = EditorGUI.DoubleField(rect2_1, "time", oldTime);
            if (!Mathf.Approximately((float)newTime, (float)oldTime))
            {
                animation.AnimateTime = TimeSpan.FromSeconds(newTime);
            }

            var oldTotalTime = animation.TotalAnimateTime.TotalSeconds;
            var newTotalTime = EditorGUI.DoubleField(rect2_2, "total", oldTotalTime);
            if (!Mathf.Approximately((float)newTotalTime, (float)oldTotalTime))
            {
                animation.TotalAnimateTime = TimeSpan.FromSeconds(newTotalTime);
            }

            EditorGUIUtility.labelWidth = lw;
        }
    }
}