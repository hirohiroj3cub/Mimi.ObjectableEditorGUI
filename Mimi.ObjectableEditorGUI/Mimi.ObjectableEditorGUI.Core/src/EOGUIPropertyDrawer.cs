using Mimi.ObjectableEditorGUI.Context;
using Mimi.ObjectableEditorGUI.Elements;
using System;
using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI
{
    public abstract class EOGUIPropertyDrawer : PropertyDrawer
    {
        private Rect position;
        private bool heightUpdate = true;
        private bool heightUpdateLate = false;

        protected EOGUIPropertyDrawer()
        {
            TopContext = new EOGUIContext(this);
            RootElement = InitialRootElement();
            InitialRootOptions(RootElement.Options);
        }

        public IEOGUIElementParent RootElement { get; set; }
        public EOGUIContext TopContext { get; }
        public bool HeightUpdate => heightUpdate;

        public virtual IEOGUIElementParent InitialRootElement()
        {
            return new EOGUIList(false)
            {
                new EOGUILabelField("Empty EOGUIPropertyDrawer")
            };
        }

        public virtual void InitialRootOptions(EOGUIElementOptions options)
        {
            options.Margin = new RectOffset(1, 1, 0, 2);
            options.BackgroundStyle = null;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            try
            {
                TopContext.Init(property, position);
                RootElement.OnPreUpdateProperty(TopContext);
                return RootElement.GetHeightProperty();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return 0f;
            }
            finally
            {
                heightUpdate = heightUpdateLate;
                heightUpdateLate = false;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.Update();
            TopContext.Init(property, position);
            RootElement.OnGUIProperty();
            property.serializedObject.ApplyModifiedProperties();
            this.position = position;
        }

        public void SetFlagOfHeightUpdateAll()
        {
            heightUpdate = true;
        }

        public void SetFlagOfHeightUpdateAllLate()
        {
            heightUpdateLate = true;
        }
    }
}