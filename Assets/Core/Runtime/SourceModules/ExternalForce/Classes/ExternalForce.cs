using UnityEngine;

namespace Runtime.SourceModules.ExternalForce
{
    public abstract class ExternalForce : ScriptableObject
    {
        public abstract void Impact(ref Vector3 velocity, float deltaTime);
    }
}