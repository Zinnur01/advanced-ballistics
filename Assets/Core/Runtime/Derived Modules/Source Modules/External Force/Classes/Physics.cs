using UnityEngine;

namespace Runtime.SourceModules.ExternalForce
{
    [CreateAssetMenu(fileName = "Physics", menuName = "External Forces/Physics")]
    public class Physics : ExternalForce
    {
        [SerializeField]
        private float multiplier = 1f;

        public override void Impact(ref Vector3 velocity, float deltaTime)
        {
            velocity += UnityEngine.Physics.gravity * (multiplier * deltaTime);
        }
    }
}