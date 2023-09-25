using System.Collections.Generic;

namespace Mimi.ObjectableEditorGUI.Context
{
    internal static class DictionaryEx
    {
        public static void AddRange(this Dictionary<object, object> @this, IEnumerable<KeyValuePair<object, object>> items)
        {
            foreach (var item in items)
            {
                @this[item.Key] = item.Value;
            }
        }
    }
}