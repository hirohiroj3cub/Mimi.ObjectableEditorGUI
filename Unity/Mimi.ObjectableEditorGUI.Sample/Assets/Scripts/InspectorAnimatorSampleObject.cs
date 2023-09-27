using UnityEngine;

namespace Mimi.InspectorAnimation.Sample
{
    [CreateAssetMenu(menuName = "Mimi.InspectorAnimator.Sample/SampleObject")]
    public class InspectorAnimatorSampleObject : ScriptableObject
    {
        [SerializeField]
        private AnimationTest animationTest;
        [SerializeField]
        private AnimationTest animationTest2;
    }
}
