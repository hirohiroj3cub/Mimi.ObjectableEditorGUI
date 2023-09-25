using System;
using System.Collections.Generic;

namespace Mimi.ObjectableEditorGUI.Conparers
{
    public class ArrayEqualityComparer : IEqualityComparer<object>
    {
        public static ArrayEqualityComparer Instance { get; } = new ArrayEqualityComparer();

        private ArrayEqualityComparer()
        {
        }

        public new bool Equals(object x, object y)
        {
            if (x == null || y == null)
                return x == y;

            if (x is Array xArray && y is Array yArray)
            {
                if (xArray.Length != yArray.Length)
                    return false;

                for (int i = 0; i < xArray.Length; i++)
                {
                    if (!object.Equals(xArray.GetValue(i), yArray.GetValue(i)))
                        return false;
                }

                return true;
            }
            else
            {
                return x.Equals(y);
            }
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }
    }
}