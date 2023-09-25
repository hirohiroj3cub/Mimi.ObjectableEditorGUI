using Mimi.ObjectableEditorGUI.Context;
using UnityEditor;
using UnityEditorInternal;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIReorderableProperty : EOGUIReorderableProperty<EOGUIReorderableProperty, EOGUIReorderablePropertyElement>
    {
        public EOGUIReorderableProperty(EOGUIContextWriterSerializedProperty serializedPropertyWriter) : base(serializedPropertyWriter)
        {
        }

        protected override EOGUIReorderablePropertyElement CreateNewElement()
        {
            return new EOGUIReorderablePropertyElement();
        }

    }

    public abstract class EOGUIReorderableProperty<TSelf, TElement> : EOGUIReorderable<TSelf, TElement>
        where TSelf : EOGUIReorderableProperty<TSelf, TElement>
        where TElement : class, IEOGUIElementChild
    {
        private readonly EOGUIContextWriterSerializedProperty serializedPropertyWriter;

        protected EOGUIReorderableProperty(EOGUIContextWriterSerializedProperty serializedPropertyWriter) : base()
        {
            this.serializedPropertyWriter = serializedPropertyWriter ?? new EOGUIContextWriterSerializedProperty();
        }

        public override void OnElementContextUpdate()
        {
            base.OnElementContextUpdate();
            serializedPropertyWriter.Write(this, Status);
            UpdateSerializedProperty();

            var newSerializedProperty = Status.SerializedProperty;
            ReorderableList.serializedProperty = newSerializedProperty;

            newSerializedProperty.serializedObject.Update();

            var elementsCount = Elements.Count;
            var propertyCount = newSerializedProperty.arraySize;
            if (elementsCount < propertyCount)
            {
                for (int i = elementsCount; i < propertyCount; i++)
                {
                    base.Add(CreateNewElement());
                }

                Context?.Drawer.SetFlagOfHeightUpdateAll();
            }
            else if (elementsCount > propertyCount)
            {
                for (int i = propertyCount; i < elementsCount; i++)
                {
                    base.RemoveAt(Elements.Count - 1);
                }

                Context?.Drawer.SetFlagOfHeightUpdateAll();
            }
        }

        public override void Add(TElement item)
        {
            base.Add(item);
            SerializedProperty?.InsertArrayElementAtIndex(SerializedProperty.arraySize);
        }

        public override void Insert(int index, TElement item)
        {
            base.Insert(index, item);
            SerializedProperty?.InsertArrayElementAtIndex(index);
        }

        public override bool Remove(TElement item)
        {
            var index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void RemoveAt(int index)
        {
            SerializedProperty?.DeleteArrayElementAtIndex(index);
            base.RemoveAt(index);
        }

        public override void Clear()
        {
            SerializedProperty?.ClearArray();
            base.Clear();
        }
    }
}