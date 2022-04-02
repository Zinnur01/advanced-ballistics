using ApexInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBullet : PoolObject
{
    [SerializeField]
    [Suffix("ì/ñ")]
    private int initialSpeed = 715;

    [SerializeField]
    private PoolObject decalTemplate;

    [SerializeField]
    private LayerMask cullingLayer;

    [SerializeField]
    private Gradient gradient;

    // Stored required properties.
    [SerializeField]
    [ReadOnly]
    private Vector3 velocity;

    private Vector3 lastPosition;

    private void FixedUpdate()
    {
        lastPosition = transform.position;

        velocity += Physics.gravity * Time.fixedDeltaTime;
        transform.Translate(velocity * Time.fixedDeltaTime, Space.World);

        if (PhysicsExtension.LinecastBoth(lastPosition, transform.position, out RaycastBothHit bothHitInfo))
        {
            CreateDecal(bothHitInfo.inHit);
            transform.position = bothHitInfo.inHit.point;
            Debug.DrawLine(lastPosition, transform.position, GetVelocityColor(), 10);

            PhysicsSurface surface = bothHitInfo.inHit.transform.GetComponent<PhysicsSurface>();
            if (surface != null)
            {
                float neededBulletPenetrationForce = PenetrationStrength(bothHitInfo.gap, surface.GetSurface().GetStrength());

                if (velocity.magnitude > neededBulletPenetrationForce)
                {
                    CreateDecal(bothHitInfo.outHit);
                    transform.position = bothHitInfo.outHit.point;
                    velocity += -velocity.normalized * neededBulletPenetrationForce;
                }
                else
                {
                    Push();
                }
            }
            else
            {
                Push();
            }
        }
        else
        {
            Debug.DrawLine(lastPosition, transform.position, GetVelocityColor(), 10);
        }
        //if (Physics.Linecast(lastPosition, transform.position, out RaycastHit hitInfo, cullingLayer))
        //{
        //    CreateDecal(hitInfo);
        //    Debug.DrawLine(lastPosition, transform.position, Color.red, 10);

        //    if (Physics.Linecast(hitInfo.point + velocity.normalized * penetration, lastPosition, out RaycastHit penetrationHitInfo, cullingLayer))
        //    {
        //        CreateDecal(penetrationHitInfo);
        //        Debug.DrawLine(hitInfo.point + velocity.normalized * penetration, lastPosition, Color.green, 10);

        //        velocity *= 0.5f;
        //    }
        //    else
        //    {
        //        transform.position = hitInfo.point;
        //        float dot = 1f - Vector3.Dot(hitInfo.normal, -velocity.normalized);
        //        velocity = Vector3.Reflect(velocity, hitInfo.normal) * dot;
        //    }

        //    if (velocity.sqrMagnitude < 1)
        //    {
        //        Push();
        //    }
        //}
    }

    private Color GetVelocityColor()
    {
        float a = velocity.magnitude / 500f;
        return gradient.Evaluate(Mathf.Min(a, 1));
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

    private float PenetrationStrength(float thickness, float strength)
    {
        return thickness * strength;
    }
}

//public class DensityMaterial : MonoBehaviour
//{
//    [SerializeField]
//    private Density density;
//}

//public class Density : ScriptableObject
//{
//    [SerializeField]
//    private float density;

//    [SerializeField]
//    private float temperature;
//}

//public class Atmosphere : PhysicMaterial
//{
//    public float strength;
//}