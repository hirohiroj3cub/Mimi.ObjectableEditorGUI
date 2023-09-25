using System;

namespace Mimi.ObjectableEditorGUI
{
    public static class EOGUIElementInvoker_Extensions
    {
        public static void Invoke<TInvoker, T1, T2>(this TInvoker invoker, T1 element1, T2 element2,
            EOGUIElementInvoker<TInvoker>.EventHandler<T1, T2> handler1, EOGUIElementInvoker<TInvoker>.EventHandler<T2> handler2)
            where TInvoker : EOGUIElementInvoker<TInvoker>
            where T1 : class, IEOGUIElement
            where T2 : class, IEOGUIElement
        {
            if (handler1 == null)
            {
                handler2?.Invoke(element2, invoker);
            }
            else
            {
                var chain = handler2 ?? EOGUIElementInvoker<TInvoker>.DefaultHandlers<T2>.Value;
                handler1(element1, invoker, chain);
            }
        }

        public static EOGUIElementInvoker<TInvoker>.EventHandler<T> Chain<TInvoker, T>(
            this EOGUIElementInvoker<TInvoker>.EventHandler<T> handler1, EOGUIElementInvoker<TInvoker>.EventHandler<T, T> handler2)
            where TInvoker : EOGUIElementInvoker<TInvoker>
            where T : class, IEOGUIElement
        {
            return Chain2(handler1, handler2);
        }

        public static EOGUIElementInvoker<TInvoker>.EventHandler<T1> Chain2<TInvoker, T1, T2>(
            this EOGUIElementInvoker<TInvoker>.EventHandler<T2> handler1, EOGUIElementInvoker<TInvoker>.EventHandler<T1, T2> handler2)
            where TInvoker : EOGUIElementInvoker<TInvoker>
            where T1 : class, IEOGUIElement
            where T2 : class, IEOGUIElement
        {
            if (handler2 is null)
            {
                throw new ArgumentNullException(nameof(handler2));
            }

            if (handler1 is null)
            {
                return (element, invoker) =>
                {
                    handler2.Invoke(element, invoker, EOGUIElementInvoker<TInvoker>.DefaultHandlers<T2>.Value);
                };
            }
            else
            {
                return (element, invoker) =>
                {
                    handler2.Invoke(element, invoker, handler1);
                };
            }
        }
    }
}