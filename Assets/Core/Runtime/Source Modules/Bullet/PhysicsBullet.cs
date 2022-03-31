using ApexInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBullet : PoolObject
{
    [SerializeField]
    [Suffix("Ï/Ò")]
    private int initialSpeed = 715;

    //[SerializeField]
    //[Suffix("Í„Ò*Ò")]
    //private float impulse = 1.25f;

    //[SerializeField]
    //private float mass = 0.008f;

    [SerializeField]
    private float penetration = 0.01f;

    [SerializeField]
    private PoolObject decalTemplate;

    [SerializeField]
    private LayerMask cullingLayer;

    // Stored required properties.
    [SerializeField]
    [ReadOnly]
    private Vector3 velocity;

    private Vector3 lastPosition;

    private void Start()
    {
        penetration += 0.001f;
    }

    private void FixedUpdate()
    {
        lastPosition = transform.position;

        velocity += Physics.gravity * Time.fixedDeltaTime;
        transform.Translate(velocity * Time.fixedDeltaTime, Space.World);

        if (Physics.Linecast(lastPosition, transform.position, out RaycastHit hitInfo, cullingLayer))
        {
            CreateDecal(hitInfo);
            Debug.DrawLine(lastPosition, transform.position, Color.red, 10);

            if (Physics.Linecast(hitInfo.point + velocity.normalized * penetration, lastPosition, out RaycastHit penetrationHitInfo, cullingLayer))
            {
                CreateDecal(penetrationHitInfo);
                Debug.DrawLine(hitInfo.point + velocity.normalized * penetration, lastPosition, Color.green, 10);
            }
            else
            {
                transform.position = hitInfo.point;
                float dot = 1f - Vector3.Dot(hitInfo.normal, -velocity.normalized);
                velocity = Vector3.Reflect(velocity, hitInfo.normal) * dot;
                if (velocity.sqrMagnitude < 1)
                {
                    Push();
                }
            }
        }
    }

    private void CreateDecal(RaycastHit hitInfo)
    {
        if (velocity.sqrMagnitude > 100)
        {
            PoolManager.Instance.CreateOrPop<Decal>(decalTemplate, hitInfo.point, Quaternion.LookRotation(-hitInfo.normal, Vector3.up));
        }
    }

    public void ApplyVelocity(Vector3 vector)
    {
        velocity = vector * initialSpeed;
    }
}

public class DensityMaterial : MonoBehaviour
{
    [SerializeField]
    private Density density;
}

public class Density : ScriptableObject
{
    [SerializeField]
    private float density;

    [SerializeField]
    private float temperature;
}

public class Atmosphere
{

}