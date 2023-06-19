using UnityEngine;

namespace Runtime.SourceModules.ExternalForce
{
    [CreateAssetMenu(fileName = "Air Resistance", menuName = "External Forces/Air Resistance")]
    public class AirResistance : ExternalForce
    {
        [SerializeField]
        private float ratio = 0.1f;

        public override void Compute(ref Vector3 velocity, float deltaTime)
        {
            velocity += -velocity * (deltaTime * ratio);
        }
    }
}