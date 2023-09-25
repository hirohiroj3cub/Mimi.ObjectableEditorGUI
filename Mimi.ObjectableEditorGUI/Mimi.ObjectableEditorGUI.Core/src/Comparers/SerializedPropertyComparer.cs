using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Mimi.ObjectableEditorGUI.Conparers
{
    public abstract class SerializedPropertyComparer : IEqualityComparer, IEqualityComparer<SerializedProperty>
    {
        public static SerializedPropertyComparer Contents { get; } = new ContentsComparer();

        public new bool Equals(object x, object y)
        {
            if (x == y) return true;

            return x != null && y != null && (x, y) switch
            {
                (SerializedProperty xp, SerializedProperty yp) => Equals(xp, yp),
                _ => x.Equals(y)
            };
        }

        public int GetHashCode(object obj)
        {
            return obj switch
            {
                null => throw new ArgumentNullException(nameof(obj)),
                SerializedProperty property => GetHashCode(property),
                _ => obj.GetHashCode()
            };
        }

        public abstract bool Equals(SerializedProperty x, SerializedProperty y);

        public abstract int GetHashCode(SerializedProperty obj);

        private class ContentsComparer : SerializedPropertyComparer
        {
            public override bool Equals(SerializedProperty x, SerializedProperty y)
            {
                return (x == y) || (x != null && y != null && SerializedProperty.EqualContents(x, y));
            }

            public override int GetHashCode(SerializedProperty obj)
            {
                return HashCode.Combine(obj.propertyPath, obj.serializedObject?.targetObject);
            }
        }
    }
}