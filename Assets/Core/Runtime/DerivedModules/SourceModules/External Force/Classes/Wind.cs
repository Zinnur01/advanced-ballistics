using Runtime.Attributes;
using System;
using UnityEngine;

namespace Runtime.SourceModules.ExternalForce
{
    [CreateAssetMenu(fileName = "Wind", menuName = "External Forces/Wind")]
    public class Wind : ExternalForce
    {
        [SerializeField]
        [Direction(normalized = true)]
        private UnityEngine.Vector2 direction;

        [SerializeField]
        [Min(0f)]
        private float force;

        private Vector3 velocity => new Vector3(direction.x, 0, direction.y) * force;

        public override void Compute(ref Vector3 velocity, float deltaTime)
        {
            velocity += this.velocity * deltaTime;
        }

        private void OnValidate()
        {
            OnChangeVelocity?.Invoke(velocity);
        }

        #region [Events]
        public event Action<Vector3> OnChangeVelocity;
        #endregion

        #region [Getter / Setter]
        public Vector3 GetVelocity()
        {
            return velocity;
        }
        #endregion
    }
}