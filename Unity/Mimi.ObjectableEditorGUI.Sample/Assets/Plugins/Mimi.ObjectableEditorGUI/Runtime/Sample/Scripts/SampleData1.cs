using System;
using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Sample
{
    [Serializable]
    public class SampleDataA
    {
        [SerializeField]
        public int intValue;
        [SerializeField]
        public string stringValue;
        [SerializeField]
        public float minValue;
        [SerializeField]
        public float maxValue;
        [SerializeField]
        public float rangeValue;
        [SerializeField]
        public int[] arrayValue;
        [SerializeField]
        public SampleDataC[] arrayDataC;

        public override string ToString()
        {
            return $"{intValue}, {stringValue}, {minValue}, {maxValue}, {rangeValue}";
        }
    }

    [Serializable]
    public class SampleDataC
    {
        public string text;
        public int num;
    }

    [Serializable]
    public class SampleDataB
    {
        [SerializeField]
        public SampleDataA sampleDataA1;
        //[SerializeField]
        //public SampleDataA sampleDataA2;
    }
}
