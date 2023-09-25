using Mimi.ObjectableEditorGUI.Context;
using Mimi.ObjectableEditorGUI.GUIUtility;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI
{
    public static class IEOGUIElement_Extensions
    {
        public static bool HasContext(this IEOGUIElement element) => element.Context != null;

        public static bool HasContextAndIsDirty(this IEOGUIElement element) => element.Context != null && element.IsDirty;

        public static string GetNamePath(this IEOGUIElement element)
        {
            var stringBuilder = new StringBuilder();

            while (element != null)
            {
                stringBuilder.Insert(0, element.Name);
                element = element.Parent;
                if (element != null)
                {
                    stringBuilder.Insert(0, "/");
                }
            }

            return stringBuilder.ToString();
        }

        private static void CacheStatus(this IEOGUIElement element)
        {
            element.ExtensionsResource.cachedStatus.Set(element.Status);
        }

        private static void GetCachedStatus(this IEOGUIElement element, bool useCachedSize)
        {
            element.Status.Set(element.ExtensionsResource.cachedStatus, useCachedSize ? element : null);
        }

        public static void OnPreUpdateElements(this IEOGUIElementParent parent, EOGUIContext context)
        {
            OnPreUpdateElements(parent.Cast<IEOGUIElement>(), context);
        }

        public static void OnPreUpdateElements(this IEnumerable<IEOGUIElement> elements, EOGUIContext context)
        {
            foreach (var element in elements)
            {
                element.BeginElement();
                element.OnPreUpdate(context);
            }
        }

        public static void OnPreUpdateProperty(this IEOGUIElement element, EOGUIContext context)
        {
            element.BeginElement();
            element.OnPreUpdate(context);
        }

        private static void OnPreUpdate(this IEOGUIElement element, EOGUIContext context)
        {
            ref var elementContext = ref element.ExtensionsResource.context;
            if (elementContext == null || elementContext != context)
            {
                elementContext = context;
            }

            using var contextScope = new EOGUIContext.Scope(context);

            ref readonly float cachedWidth = ref element.ExtensionsResource.cachedWidth;
            ref readonly float cachedHeight = ref element.ExtensionsResource.cachedHeight.current;
            context.Status.rect = new Rect(0, 0, cachedWidth, cachedHeight);

            element.Options.CallCustomContextUpdate(element);
            element.CacheStatus();
            element.Options.CallCustomPreUpdate(element);
        }

        public static float GetHeightElements(this IEOGUIElementParent parent)
        {
            return GetHeightElements(parent.Cast<IEOGUIElement>(), parent.IsHorizontal);
        }

        public static float GetHeightElements(this IEnumerable<IEOGUIElement> elements, bool isHorizontal)
        {
            float height = 0;
            if (isHorizontal)
            {
                foreach (var e in elements)
                {
                    float newHeight = e.GetHeightProperty();
                    if (newHeight > height) height = newHeight;
                }
            }
            else
            {
                foreach (var e in elements)
                {
                    height += e.GetHeightProperty();
                }
            }

            return height;
        }

        public static float GetHeightProperty(this IEOGUIElement element)
        {
            var options = element.Options;
            if (!options.Visible) return 0;

            return element.GetHeight() + options.Padding.vertical + options.Margin.vertical;
        }

        private static float GetHeight(this IEOGUIElement element)
        {
            element.GetCachedStatus(true);

            ref readonly var cachedHeight = ref element.ExtensionsResource.cachedHeight;

            if (!cachedHeight.isInitialized)
            {
                cachedHeight.isInitialized = true;
                var target = element.GetElementHeight();
                cachedHeight.current = target;
                cachedHeight.target = target;
                return target;
            }

            if (element.HeightType is EOGUIHeightType.Static)
            {
                if (element.Context.Drawer.HeightUpdate)
                {
                    var newTarget = element.GetElementHeight();
                    cachedHeight.current = newTarget;
                    cachedHeight.target = newTarget;
                }
                else if (cachedHeight.current != cachedHeight.target)
                {
                    cachedHeight.current = cachedHeight.target;
                }
            }
            else
            {
                var animator = element.ExtensionsResource.animator;

                animator.OnGUIUpdate(element.Status.SerializedProperty.serializedObject);
                if (element.Context.Drawer.HeightUpdate)
                {
                    float newTarget = element.GetElementHeight();
                    if (cachedHeight.target != newTarget)
                    {
                        element.Context.Drawer.SetFlagOfHeightUpdateAllLate();
                    }
                    cachedHeight.target = newTarget;
                }

                if (Mathf.Abs(cachedHeight.target - cachedHeight.current) >= 2f)
                {
                    if (!animator.IsPlaying) animator.Start();

                    float def = cachedHeight.target - cachedHeight.current;
                    def = Mathf.Sign(def) * Mathf.Min(10f, 0.1f + Mathf.Abs(def) * 0.15f);
                    cachedHeight.current += def;
                }
                else
                {
                    cachedHeight.current = cachedHeight.target;
                    if (animator.IsPlaying) animator.Stop();
                }
            }

            return cachedHeight.current;
        }

        public static void OnGUIElements<TParent>(this IEOGUIElementParent parent)
            where TParent : IEOGUIElementParent
        {
            OnGUIElements(parent.Cast<IEOGUIElement>(), parent.IsHorizontal);
        }

        public static void OnGUIElements(this IEnumerable<IEOGUIElement> elements, bool isHorizontal)
        {
            elements = elements.Where(e => e.Options.Visible).ToArray();

            if (isHorizontal)
            {
                var arg = new OnPreUpdateHorizontalArgs(elements);
                float sumWidth = 0f;
                foreach (var element in elements)
                {
                    element.OnGUIHorizontal(arg, ref sumWidth);
                    element.EndElement();
                }
            }
            else
            {
                float sumHeight = 0f;
                foreach (var element in elements)
                {
                    element.OnGUIVertical(ref sumHeight);
                    element.EndElement();
                }
            }

        }

        public static void OnGUIProperty(this IEOGUIElement element)
        {
            var options = element.Options;

            if (!options.Visible) return;

            bool isHorizontal = element.Parent?.IsHorizontal ?? false;
            if (isHorizontal)
            {
                var arg = new OnPreUpdateHorizontalArgs(element);
                float sumWidth = 0f;
                element.OnGUIHorizontal(arg, ref sumWidth);
                element.EndElement();
            }
            else
            {
                float sumHeight = 0f;
                element.OnGUIVertical(ref sumHeight);
                element.EndElement();
            }
        }

        private static void OnGUIVertical(this IEOGUIElement element, ref float sumHeight)
        {
            ref var rect = ref element.Status.rect;
            var oldRect = new Rect(rect);

            element.GetCachedStatus(false);

            element.ElementSizeVertical(ref sumHeight, out var backgroundRect);

            using var elementGUIScope = new OnGUIScope(element, ref backgroundRect, ref rect);

            element.Options.CallCustomGUI(element);

            rect = oldRect;
        }

        private static void OnGUIHorizontal(this IEOGUIElement element, in OnPreUpdateHorizontalArgs arg, ref float sumWidth)
        {
            ref var rect = ref element.Status.rect;
            var oldRect = new Rect(rect);

            element.GetCachedStatus(false);

            ElementSizeHorizontal(element, arg, ref sumWidth, out var backgroundRect, out float labelWidth);

            using var elementGUIScope = new OnGUIScope(element, ref backgroundRect, ref rect);
            using var labelWidthScope = new GUILabelWidthScope(labelWidth);
            using var IndentScope = new GUIDynamicIndentScope(1);

            element.Options.CallCustomGUI(element);

            rect = oldRect;
        }

        private static void ElementSizeVertical(this IEOGUIElement element, ref float sumHeight, out Rect backgroundRect)
        {
            var options = element.Options;

            if (!options.Visible)
            {
                backgroundRect = new Rect();
                element.Status.rect.Set(0, 0, 0, 0);
                return;
            }
            else
            {
                ref readonly var elementHeight = ref element.ExtensionsResource.cachedHeight.current;
                ref var rect = ref element.Status.rect;
                var margin = options.Margin;
                var padding = options.Padding;
                float rectWidth = rect.width;
                float side = margin.horizontal + padding.horizontal;

                ref float elementWidth = ref element.ExtensionsResource.cachedWidth;
                if (options.WidthRatio >= 1.0f)
                {
                    elementWidth = rectWidth - side;
                }
                else
                {
                    var widthFix = Mathf.Clamp(options.WidthFix + side, 0f, rectWidth);
                    var widthRatio = Mathf.Clamp01(options.WidthRatio) * (rectWidth - widthFix);
                    elementWidth = widthFix + widthRatio;
                }

                float backgrounWidth = elementWidth + padding.horizontal;

                backgroundRect = new Rect(
                    options.GetAlignSize(rect.width, backgrounWidth) + rect.x + margin.left,
                    sumHeight + rect.y + margin.top,
                    backgrounWidth,
                    elementHeight + padding.vertical);

                rect.Set(
                    backgroundRect.x + padding.left,
                    backgroundRect.y + padding.top,
                    elementWidth,
                    elementHeight);

                sumHeight += backgroundRect.height + margin.vertical;
                return;
            }
        }

        private static void ElementSizeHorizontal(this IEOGUIElement element, in OnPreUpdateHorizontalArgs args, ref float sumWidth, out Rect backgroundRect, out float labelWidth)
        {
            var options = element.Options;

            if (!options.Visible)
            {
                labelWidth = 0;
                backgroundRect = new Rect();
                element.Status.rect.Set(0, 0, 0, 0);
                return;
            }
            else
            {
                ref readonly var elementHeight = ref element.ExtensionsResource.cachedHeight.current;
                ref var rect = ref element.Status.rect;
                ref var elementWidth = ref element.ExtensionsResource.cachedWidth;
                var margin = options.Margin;
                var padding = options.Padding;
                float rectWidth = rect.width;

                float overRate = Mathf.Clamp(rectWidth / args.sumWidthFix, 0f, 1f);
                if (overRate == 0f)
                {
                    labelWidth = 0f;
                    backgroundRect = new Rect();
                    rect.Set(0, 0, 0, 0);
                    return;
                }
                else
                {
                    var widthFix = options.WidthFix + margin.horizontal + padding.horizontal;
                    if (args.sumWidthRatio > 0f && args.sumWidthFix <= rectWidth)
                    {
                        float ratiosWidth = rectWidth - args.sumWidthFix;
                        float ratioWidth = ratiosWidth * options.WidthRatio / args.sumWidthRatio;
                        labelWidth = overRate * Mathf.Min(ratioWidth + widthFix * 0.5f, widthFix);
                        elementWidth = options.WidthFix + ratioWidth;
                    }
                    else
                    {
                        labelWidth = overRate * options.WidthFix * 0.5f;
                        elementWidth = options.WidthFix;
                    }

                    float backgrounHeight = elementHeight + padding.vertical;

                    backgroundRect = new Rect(
                        sumWidth + rect.x + overRate * margin.left,
                        options.GetAlignSize(rect.height, backgrounHeight) + rect.y + margin.top,
                        overRate * (elementWidth + padding.horizontal),
                        backgrounHeight);

                    rect.Set(
                        backgroundRect.x + overRate * padding.left,
                        backgroundRect.y + padding.top,
                        overRate * elementWidth,
                        elementHeight);

                    sumWidth += overRate * (elementWidth + padding.horizontal + margin.horizontal);
                    elementWidth *= overRate;
                    return;
                }
            }
        }

        [StructLayout(LayoutKind.Auto)]
        private readonly ref struct OnPreUpdateHorizontalArgs
        {
            public readonly float sumWidthFix;
            public readonly float sumWidthRatio;

            public OnPreUpdateHorizontalArgs(IEOGUIElement element) : this()
            {
                var opt = element.Options;
                if (opt.Visible)
                {
                    sumWidthFix = opt.WidthFix + opt.Margin.horizontal + opt.Padding.horizontal;
                    sumWidthRatio = opt.WidthRatio;
                }
            }

            public OnPreUpdateHorizontalArgs(IEnumerable<IEOGUIElement> elements) : this()
            {
                foreach (var element in elements)
                {
                    var opt = element.Options;
                    if (!opt.Visible) continue;

                    sumWidthFix += opt.WidthFix + opt.Margin.horizontal + opt.Padding.horizontal;
                    sumWidthRatio += opt.WidthRatio;
                }
            }
        }

        private static readonly Vector2 clipMargin = new Vector2(10, 2);

        [StructLayout(LayoutKind.Auto)]
        private readonly ref struct OnGUIScope
        {
            private readonly EditorGUI.DisabledScope disabledScope;
            private readonly GUIColorScope colorScope;
            private readonly bool propertyScope;

            public OnGUIScope(IEOGUIElement element, ref Rect backgroundRect, ref Rect elementRect)
            {
                var options = element.Options;

                if (Event.current.type == EventType.Repaint)
                {
                    options.BackgroundStyle?.Draw(backgroundRect, GUIContent.none, -1);
                }

                var clipRect = new Rect(elementRect.position - clipMargin, elementRect.size + clipMargin + clipMargin);
                GUI.BeginGroup(clipRect);
                elementRect.position = clipMargin;

                if (options.Disabled.HasValue)
                {
                    disabledScope = new EditorGUI.DisabledScope(options.Disabled.Value);
                }
                else
                {
                    disabledScope = default;
                }

                if (options.ContentColor != null || options.BackgroundColor != null)
                {
                    colorScope = new GUIColorScope(options.ContentColor, options.BackgroundColor);
                }
                else
                {
                    colorScope = default;
                }

                var status = element.Status;

                if (options.UsePropertyScope && status.SerializedProperty != null)
                {
                    propertyScope = true;

                    var propertyRect = new Rect(0, 0, elementRect.width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.BeginProperty(propertyRect, status.ValidLabel, status.SerializedProperty);
                }
                else
                {
                    propertyScope = false;
                }
            }

            public void Dispose()
            {
                if (propertyScope)
                {
                    EditorGUI.EndProperty();
                }

                colorScope.Dispose();
                disabledScope.Dispose();

                GUI.EndGroup();
            }
        }
    }
}