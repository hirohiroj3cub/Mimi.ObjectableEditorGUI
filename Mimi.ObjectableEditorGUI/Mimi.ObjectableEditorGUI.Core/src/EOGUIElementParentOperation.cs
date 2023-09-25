using System;

namespace Mimi.ObjectableEditorGUI
{
    public abstract class EOGUIElementParentOperation<TElement>
        where TElement : IEOGUIElementChild
    {
        public abstract void Invoke(IEOGUIElementParent<TElement> self);

        public sealed class Add : EOGUIElementParentOperation<TElement>
        {
            public Func<IEOGUIElementParent<TElement>, TElement> Func { get; }

            public Add(Func<IEOGUIElementParent<TElement>, TElement> func) => Func = func;

            public override void Invoke(IEOGUIElementParent<TElement> self)
            {
                if (Func != null)
                {
                    self.Add(Func.Invoke(self));
                }
            }
        }

        public sealed class Insert : EOGUIElementParentOperation<TElement>
        {
            public Func<IEOGUIElementParent<TElement>, (int, TElement)> Func { get; }

            public Insert(Func<IEOGUIElementParent<TElement>, (int, TElement)> func) => Func = func;

            public override void Invoke(IEOGUIElementParent<TElement> self)
            {
                if (Func != null)
                {
                    (var i, var e) = Func(self);
                    self.Insert(i, e);
                }
            }
        }

        public sealed class Remove : EOGUIElementParentOperation<TElement>
        {
            public Func<IEOGUIElementParent<TElement>, TElement, bool> Func { get; }

            public Remove(Func<IEOGUIElementParent<TElement>, TElement, bool> func) => Func = func;

            public override void Invoke(IEOGUIElementParent<TElement> self)
            {
                if (Func != null)
                {
                    var list = self.AsList;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (Func.Invoke(self, list[i]))
                        {
                            list.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }
    }
}