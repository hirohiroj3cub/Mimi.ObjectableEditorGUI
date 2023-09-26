using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIButton : EOGUIButton<EOGUIButton>
    {
        public EOGUIButton() : base()
        {
        }

        public EOGUIButton(GUIContent label) : base(label)
        {
        }

        public EOGUIButton(string labelText) : base(labelText)
        {
        }
    }

    public abstract class EOGUIButton<TSelf> : EOGUIElement<TSelf>, IEnumerable<EOGUIElementEventHandler<TSelf>>
        where TSelf : EOGUIButton<TSelf>
    {
        public EOGUIButton()
        {
            Options = new EOGUIElementOptions<TSelf>()
            {
                Margin = new RectOffset(2, 2, 2, 2),
            };

            Actions = new List<EOGUIElementEventHandler<TSelf>>();
        }

        public EOGUIButton(GUIContent label) : this()
        {
            Label = label;
        }

        public EOGUIButton(string labelText) : this()
        {
            Label = new GUIContent(labelText);
        }

        public override EOGUIHeightType HeightType => EOGUIHeightType.Static;
        public EOGUIElementEventHandler<TSelf> Action
        {
            get => Actions.SingleOrDefault();
            set
            {
                Actions.Clear();
                Actions.Add(value);
            }
        }
        public List<EOGUIElementEventHandler<TSelf>> Actions { get; }

        public override float GetElementHeight()
        {
            return GUI.skin.button.CalcSize(ValidLabel).y;
        }

        public override void OnElementGUI()
        {
            var label = ValidLabel;

            if (GUI.Button(Status.rect, label))
            {
                if (Actions.Count == 0)
                {
                    Debug.Log($"Push {label.text} ({Name}).");
                }
                else
                {
                    foreach (var action in Actions)
                    {
                        action?.Invoke(ThisT);
                    }
                }
            }
        }

        public EOGUIElementEventHandler<TSelf>? Add(Action action)
        {
            if (action == null) return null;

            void Action(TSelf element) => action();

            Actions.Add(Action);
            return Action;
        }

        public EOGUIElementEventHandler<TSelf>? Add(EOGUIElementEventHandler<TSelf> action)
        {
            if (action == null) return null;

            Actions.Add(action);
            return action;
        }

        public bool Remove(EOGUIElementEventHandler<TSelf> action)
        {
            return Actions.Remove(action);
        }

        public IEnumerator<EOGUIElementEventHandler<TSelf>> GetEnumerator()
        {
            return Actions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Actions.GetEnumerator();
        }
    }
}