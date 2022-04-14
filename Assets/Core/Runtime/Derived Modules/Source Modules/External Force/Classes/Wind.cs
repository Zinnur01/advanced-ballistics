using System;
using UnityEngine;

namespace Runtime.SourceModules.ExternalForce
{
    [CreateAssetMenu(fileName = "Wind", menuName = "External Forces/Wind")]
    public class Wind : ExternalForce
    {
        [SerializeField]
        private Vector3 velocity = new Vector3(0, 0, 10);

        public event Action<Vector3> OnChangeVelocity;

        public override void Impact(ref Vector3 velocity, float deltaTime)
        {
            velocity += this.velocity * deltaTime;
        }

        private void OnValidate()
        {
            OnChangeVelocity?.Invoke(velocity);
        }

        #region [Getter / Setter]
        public Vector3 GetVelocity()
        {
            return velocity;
        }

        public void SetVelocity(Vector3 velocity)
        {
            this.velocity = velocity;
            OnChangeVelocity?.Invoke(velocity);
        }
        #endregion
    }
}