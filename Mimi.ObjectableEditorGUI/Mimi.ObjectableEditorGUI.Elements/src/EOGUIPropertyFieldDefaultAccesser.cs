using System;
using UnityEditor;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public static class EOGUIPropertyFieldDefaultAccesser<T>
    {
        public static Func<SerializedProperty, T>? Getter { get; set; }
        public static Action<SerializedProperty, T>? Setter { get; set; }

        static EOGUIPropertyFieldDefaultAccesser()
        {
            Init();
        }

        private static void Init()
        {
            var type = typeof(T);
            if (type == typeof(AnimationCurve))
            {
                EOGUIPropertyFieldDefaultAccesser<AnimationCurve>.Getter = (prop) => prop.animationCurveValue;
                EOGUIPropertyFieldDefaultAccesser<AnimationCurve>.Setter = (prop, value) => prop.animationCurveValue = value;
            }
            else if (type == typeof(bool))
            {
                EOGUIPropertyFieldDefaultAccesser<bool>.Getter = (prop) => prop.boolValue;
                EOGUIPropertyFieldDefaultAccesser<bool>.Setter = (prop, value) => prop.boolValue = value;
            }
            else if (type == typeof(BoundsInt))
            {
                EOGUIPropertyFieldDefaultAccesser<BoundsInt>.Getter = (prop) => prop.boundsIntValue;
                EOGUIPropertyFieldDefaultAccesser<BoundsInt>.Setter = (prop, value) => prop.boundsIntValue = value;
            }
            else if (type == typeof(Bounds))
            {
                EOGUIPropertyFieldDefaultAccesser<Bounds>.Getter = (prop) => prop.boundsValue;
                EOGUIPropertyFieldDefaultAccesser<Bounds>.Setter = (prop, value) => prop.boundsValue = value;
            }
            else if (type == typeof(Color))
            {
                EOGUIPropertyFieldDefaultAccesser<Color>.Getter = (prop) => prop.colorValue;
                EOGUIPropertyFieldDefaultAccesser<Color>.Setter = (prop, value) => prop.colorValue = value;
            }
            else if (type == typeof(double))
            {
                EOGUIPropertyFieldDefaultAccesser<double>.Getter = (prop) => prop.doubleValue;
                EOGUIPropertyFieldDefaultAccesser<double>.Setter = (prop, value) => prop.doubleValue = value;
            }
            else if (type == typeof(float))
            {
                EOGUIPropertyFieldDefaultAccesser<float>.Getter = (prop) => prop.floatValue;
                EOGUIPropertyFieldDefaultAccesser<float>.Setter = (prop, value) => prop.floatValue = value;
            }
            else if (type == typeof(Hash128))
            {
                EOGUIPropertyFieldDefaultAccesser<Hash128>.Getter = (prop) => prop.hash128Value;
                EOGUIPropertyFieldDefaultAccesser<Hash128>.Setter = (prop, value) => prop.hash128Value = value;
            }
            else if (type == typeof(int))
            {
                EOGUIPropertyFieldDefaultAccesser<int>.Getter = (prop) => prop.intValue;
                EOGUIPropertyFieldDefaultAccesser<int>.Setter = (prop, value) => prop.intValue = value;
            }
            else if (type == typeof(long))
            {
                EOGUIPropertyFieldDefaultAccesser<long>.Getter = (prop) => prop.longValue;
                EOGUIPropertyFieldDefaultAccesser<long>.Setter = (prop, value) => prop.longValue = value;
            }
            else if (typeof(UnityEngine.Object).IsSubclassOf(type))
            {
                EOGUIPropertyFieldDefaultAccesser<UnityEngine.Object>.Getter = (prop) => prop.objectReferenceValue;
                EOGUIPropertyFieldDefaultAccesser<UnityEngine.Object>.Setter = (prop, value) => prop.objectReferenceValue = value;
            }
            else if (type == typeof(Quaternion))
            {
                EOGUIPropertyFieldDefaultAccesser<Quaternion>.Getter = (prop) => prop.quaternionValue;
                EOGUIPropertyFieldDefaultAccesser<Quaternion>.Setter = (prop, value) => prop.quaternionValue = value;
            }
            else if (type == typeof(RectInt))
            {
                EOGUIPropertyFieldDefaultAccesser<RectInt>.Getter = (prop) => prop.rectIntValue;
                EOGUIPropertyFieldDefaultAccesser<RectInt>.Setter = (prop, value) => prop.rectIntValue = value;
            }
            else if (type == typeof(Rect))
            {
                EOGUIPropertyFieldDefaultAccesser<Rect>.Getter = (prop) => prop.rectValue;
                EOGUIPropertyFieldDefaultAccesser<Rect>.Setter = (prop, value) => prop.rectValue = value;
            }
            else if (type == typeof(string))
            {
                EOGUIPropertyFieldDefaultAccesser<string>.Getter = (prop) => prop.stringValue;
                EOGUIPropertyFieldDefaultAccesser<string>.Setter = (prop, value) => prop.stringValue = value;
            }
            else if (type == typeof(Vector2Int))
            {
                EOGUIPropertyFieldDefaultAccesser<Vector2Int>.Getter = (prop) => prop.vector2IntValue;
                EOGUIPropertyFieldDefaultAccesser<Vector2Int>.Setter = (prop, value) => prop.vector2IntValue = value;
            }
            else if (type == typeof(Vector2))
            {
                EOGUIPropertyFieldDefaultAccesser<Vector2>.Getter = (prop) => prop.vector2Value;
                EOGUIPropertyFieldDefaultAccesser<Vector2>.Setter = (prop, value) => prop.vector2Value = value;
            }
            else if (type == typeof(Vector3Int))
            {
                EOGUIPropertyFieldDefaultAccesser<Vector3Int>.Getter = (prop) => prop.vector3IntValue;
                EOGUIPropertyFieldDefaultAccesser<Vector3Int>.Setter = (prop, value) => prop.vector3IntValue = value;
            }
            else if (type == typeof(Vector3))
            {
                EOGUIPropertyFieldDefaultAccesser<Vector3>.Getter = (prop) => prop.vector3Value;
                EOGUIPropertyFieldDefaultAccesser<Vector3>.Setter = (prop, value) => prop.vector3Value = value;
            }
            else if (type == typeof(Vector4))
            {
                EOGUIPropertyFieldDefaultAccesser<Vector4>.Getter = (prop) => prop.vector4Value;
                EOGUIPropertyFieldDefaultAccesser<Vector4>.Setter = (prop, value) => prop.vector4Value = value;
            }
            else if (type.IsEnum)
            {
                Getter = (prop) => (T)(object)prop.intValue;
                Setter = (prop, value) => prop.intValue = (int)(object)value!;
            }
        }
    }
}