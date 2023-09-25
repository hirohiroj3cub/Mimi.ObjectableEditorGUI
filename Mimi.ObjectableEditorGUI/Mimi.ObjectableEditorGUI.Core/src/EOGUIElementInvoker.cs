namespace Mimi.ObjectableEditorGUI
{

    public abstract class EOGUIElementInvoker<TSelf> where TSelf : EOGUIElementInvoker<TSelf>
    {
        private bool isInvoked;

        public IEOGUIElement Element { get; private set; }

        public void Invoke()
        {
            if (!isInvoked)
            {
                Invoke_Protected();
                isInvoked = true;
            }
        }

        public virtual void Finish()
        {
            if (!isInvoked) Invoke();

            isInvoked = false;
            Element = null;
        }

        public Scope Init(IEOGUIElement element)
        {
            isInvoked = false;
            Element = element;
            return new Scope(this);
        }

        protected abstract void Invoke_Protected();

        public delegate void EventHandler<TElement>(TElement element, TSelf invoker)
            where TElement : class, IEOGUIElement;

        public delegate void EventHandler<TElement, TChainElement>(TElement element, TSelf invoker, EventHandler<TChainElement> chainEvent)
            where TElement : class, IEOGUIElement
            where TChainElement : class, IEOGUIElement;

        public static class DefaultHandlers<TElement>
            where TElement : class, IEOGUIElement
        {
            public static EventHandler<TElement> Value { get; } = (e, invoker) => invoker.Invoke();
        }

        public static EventHandler<TElement> GetDefaultHandler<TElement>()
            where TElement : class, IEOGUIElement
        {
            return DefaultHandlers<TElement>.Value;
        }

        public readonly ref struct Scope
        {
            private readonly EOGUIElementInvoker<TSelf> invoker;

            public Scope(EOGUIElementInvoker<TSelf> invoker)
            {
                this.invoker = invoker;
            }

            public void Dispose()
            {
                invoker.Finish();
            }
        }
    }
}