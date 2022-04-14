using UnityEngine;

namespace Runtime.SourceModules.ExternalForce
{
    public class ExternalForcesManager : Singleton<ExternalForcesManager>
    {
        [SerializeReference]
        private ExternalForce[] forces;

        public void Impact(ref Vector3 velocity, float deltaTime)
        {
            for (int i = 0; i < forces.Length; i++)
            {
                forces[i].Impact(ref velocity, deltaTime);
            }
        }
    }
}