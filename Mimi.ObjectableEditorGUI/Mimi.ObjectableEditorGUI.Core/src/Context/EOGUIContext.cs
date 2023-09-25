using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Context
{
    public class EOGUIContext
    {
        private readonly Dictionary<object, object> customContext;
        private readonly EOGUIContextStatusStack statusStack;
        private readonly EOGUIContextStatus status;
        private readonly EOGUIPropertyDrawer drawer;

        public int Depth => statusStack.Count;
        public EOGUIPropertyDrawer Drawer => drawer;
        public EOGUIContextStatus Status => status;
        public SerializedProperty RootProperty { get; private set; }
        public Rect RootRect { get; private set; }

        public EOGUIContext(EOGUIPropertyDrawer drawer)
        {
            customContext = new Dictionary<object, object>();
            statusStack = new EOGUIContextStatusStack();
            status = new EOGUIContextStatus();
            this.drawer = drawer ?? throw new ArgumentNullException(nameof(drawer));
        }

        public void Init(SerializedProperty serializedProperty, in Rect rect)
        {
            statusStack.Init();
            RootProperty = serializedProperty ?? throw new ArgumentNullException(nameof(serializedProperty));
            RootRect = rect;
            Status.Init(serializedProperty, in rect, customContext);
        }

        public ref struct Scope
        {
            private EOGUIContext context;

            public Scope(EOGUIContext context)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                context.statusStack.Push(context.Status);
            }

            public void Dispose()
            {
                if (context != null)
                {
                    context.statusStack.Pop(context.Status);
                    context = null;
                }
            }
        }
    }
}