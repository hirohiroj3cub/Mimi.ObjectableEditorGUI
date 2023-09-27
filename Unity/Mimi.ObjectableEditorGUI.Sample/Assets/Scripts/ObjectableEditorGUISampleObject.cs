using UnityEngine;

namespace Mimi.ObjectableEditorGUI.Sample
{
    [CreateAssetMenu(menuName = "Mimi.ObjectableEditorGUI.Sample/SampleObject")]
    public class ObjectableEditorGUISampleObject : ScriptableObject
    {
        [SerializeField]
        private SampleDataB sampleDataB1;
        [SerializeField]
        private SampleDataB sampleDataB2;
    }
}
