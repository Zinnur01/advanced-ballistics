using System;
using UnityEngine;

namespace Runtime.SourceModules.ExternalForce
{
    [CreateAssetMenu(fileName = "Wind", menuName = "External Forces/Wind")]
    public class Wind : ExternalForce
    {
        public event Action<Vector3> OnChangeVelocity;

        [SerializeField]
        private Vector3 _velocity = new Vector3(0, 0, 10);

        private Vector3 velocity
        {
            get => _velocity;
            set
            {
                _velocity = value;
                OnChangeVelocity(value);
            }
        }

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
        }
        #endregion
    }
}