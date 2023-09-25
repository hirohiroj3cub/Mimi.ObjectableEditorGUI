using System;
using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIPages : EOGUIPages<EOGUIPages>
    {
        public EOGUIPages(Page[] pages) : base(pages)
        {
        }
    }

    public abstract class EOGUIPages<TSelf> : EOGUIReadOnlyList<TSelf, IEOGUIElementChild>
        where TSelf : EOGUIPages<TSelf>
    {
        public class Page
        {
            public readonly string name;
            public readonly IEOGUIElementChild element;
            public readonly bool usePropertyMenu;

            public Page(string name, IEOGUIElementChild element, bool usePropertyMenu)
            {
                this.name = name ?? throw new ArgumentNullException(nameof(name));
                this.element = element ?? throw new ArgumentNullException(nameof(element));
                this.usePropertyMenu = usePropertyMenu;
            }
        }

        public class PageSelectButtom : EOGUIElement<PageSelectButtom>
        {
            private readonly EOGUIElementWraper<IEOGUIElementChild> pageWraper;
            private readonly GUIStyle style;
            private readonly IEOGUIElementChild element;
            private readonly bool usePropertyMenu;

            public PageSelectButtom(EOGUIElementWraper<IEOGUIElementChild> pageWraper, IEOGUIElementChild element, bool usePropertyMenu)
            {
                this.pageWraper = pageWraper ?? throw new ArgumentNullException(nameof(pageWraper));
                this.element = element ?? throw new ArgumentNullException(nameof(element));
                this.usePropertyMenu = usePropertyMenu;
                style = GUI.skin.button;
                var pad = Options.Padding;
                var mag = Options.Margin;
                pad.left = 0; pad.right = 0; pad.top = 0; pad.bottom = 0;
                mag.left = 0; mag.right = 0; mag.top = 0; mag.bottom = 0;
            }

            public override EOGUIHeightType HeightType => EOGUIHeightType.Static;

            public override float GetElementHeight()
            {
                return style.CalcSize(ValidLabel).y;
            }

            public override void OnElementGUI()
            {
                var e = Event.current;
                if (e.type == EventType.Repaint)
                {
                    if (pageWraper.Element == element)
                    {
                        style.Draw(Status.rect, ValidLabel, UnityEngine.GUIUtility.GetControlID(FocusType.Passive), true);
                    }
                    else
                    {
                        style.Draw(Status.rect, ValidLabel, UnityEngine.GUIUtility.GetControlID(FocusType.Passive), false);
                    }
                }
                else if (e.type == EventType.MouseDown)
                {
                    if (e.button == 0)
                    {
                        pageWraper.DelaySet(p => element);
                    }
                    else if (usePropertyMenu && e.button == 1)
                    {
                        EditorGUI.BeginProperty(Status.rect, ValidLabel, element.SerializedProperty);
                        EditorGUI.EndProperty();
                    }
                }
            }
        }

        protected EOGUIPages(Page[] pages) : base(CreateElements(pages), false)
        {
            Options.BackgroundStyle = GUI.skin.button;
        }

        private static IEOGUIElementChild[] CreateElements(Page[] pages)
        {
            var pagesSpan = new Span<Page>(pages);
            var buttonElements = new IEOGUIElementChild[pagesSpan.Length];
            var pageWraper = new EOGUIElementWraper<IEOGUIElementChild>(pagesSpan[0].element);

            for (int i = 0; i < pagesSpan.Length; i++)
            {
                var page = pagesSpan[i];
                buttonElements[i] = new PageSelectButtom(pageWraper, page.element, page.usePropertyMenu);
            }

            var buttonsList = new EOGUIReadOnlyList(buttonElements, true);

            return new IEOGUIElementChild[]
            {
                buttonsList,
                pageWraper
            };
        }

        protected override EOGUIHeightType HeightType_Protected { get; set; } = EOGUIHeightType.Animation;
    }
}