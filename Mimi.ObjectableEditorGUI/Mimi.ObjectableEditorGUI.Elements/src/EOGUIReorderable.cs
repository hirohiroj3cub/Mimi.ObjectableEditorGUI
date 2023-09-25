using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Elements
{

    public abstract class EOGUIReorderable<TSelf, TElement> : EOGUIElementParent<TSelf, List<TElement>, TElement>
        where TSelf : EOGUIReorderable<TSelf, TElement>
        where TElement : IEOGUIElementChild
    {
        private float height;
        private string headerText = "";

        public ReorderableList ReorderableList { get; }

        public EOGUIReorderable() : base(new List<TElement>(), false)
        {
            ReorderableList = new ReorderableList(Elements, typeof(TElement))
            {
                multiSelect = true,
                drawHeaderCallback = (rect) => GUI.Label(rect, headerText),
                elementHeightCallback = (index) =>
                {
                    var element = Elements[index];
                    var pad = element.Options.Padding;
                    pad.left = 1; pad.right = 1; pad.top = 2; pad.bottom = 0;
                    var mar = element.Options.Margin;
                    mar.left = 0; mar.right = 0; mar.top = 0; mar.bottom = 0;
                    var height = element.GetHeightProperty();
                    return height;
                },
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    if (index < Elements.Count)
                    {
                        var element = Elements[index];
                        element.Status.rect = rect;
                        element.OnGUIProperty();
                    }
                },
                onAddCallback = (ReorderableList sender) =>
                {
                    var selected = sender.selectedIndices.ToArray().AsSpan();
                    if (selected.Length == 0)
                    {
                        Add(CreateNewElement());
                    }
                    else
                    {
                        var elements = Elements.ToArray().AsSpan();

                        sender.ClearSelection();

                        int count = 0;
                        foreach (var i in selected)
                        {
                            Insert(i + count, CreateNewElement(elements[i]));
                            sender.Select(i + count, true);
                            count++;
                        }
                    }
                },
                onRemoveCallback = (ReorderableList sender) =>
                {
                    var selected = sender.selectedIndices.ToArray().AsSpan();
                    var elements = Elements.ToArray().AsSpan();
                    foreach (var i in selected)
                    {
                        var element = elements[i];
                        Remove(element);
                    }

                    sender.ClearSelection();

                    foreach (var i in selected)
                    {
                        var i2 = i - selected.Length;
                        if (0 <= i2 && i2 < sender.count)
                        {
                            sender.Select(i2, true);
                        }
                    }
                },
                onReorderCallbackWithDetails = (ReorderableList sender, int oldIndex, int newIndex) =>
                {
                    (Elements[oldIndex], Elements[newIndex]) = (Elements[newIndex], Elements[oldIndex]);
                }
            };
        }

        public override EOGUIHeightType HeightType => EOGUIHeightType.Animation;

        protected abstract TElement CreateNewElement();

        protected virtual TElement CreateNewElement(TElement baseElement) => CreateNewElement();

        public override void OnElementPreUpdate()
        {
            base.OnElementPreUpdate();
            var newHeight = ReorderableList.GetHeight();
            if (height != newHeight)
            {
                Context.Drawer.SetFlagOfHeightUpdateAll();
                height = newHeight;
            }
        }

        public override float GetElementHeight()
        {
            return height;
        }

        public override void OnElementGUI()
        {
            headerText = ValidLabel.text;
            ReorderableList.DoList(Status.rect);
        }
    }
}