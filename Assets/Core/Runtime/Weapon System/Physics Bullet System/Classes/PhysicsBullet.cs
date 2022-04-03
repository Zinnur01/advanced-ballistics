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
    private float mass = 0.003f;

    [SerializeField]
    private PoolObject decalTemplate;

    [SerializeField]
    private LayerMask cullingLayer;

    [SerializeField]
    private Gradient gradient;

    // Stored required properties.
    private Vector3 lastPosition;
    private Vector3 velocity;

    public void Initialize(Vector3 vector)
    {
        velocity = vector * initialSpeed;
    }

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

            Through(bothHitInfo);
        }
        else
        {
            Debug.DrawLine(lastPosition, transform.position, GetVelocityColor(), 10);
        }
    }

    private void Through(RaycastBothHit bothHit)
    {
        PhysicsSurface surface = bothHit.inHit.transform.GetComponent<PhysicsSurface>();
        if (surface != null)
        {
            float neededBulletPenetrationForce = PenetrationStrength(bothHit.gap, surface.GetSurface().GetStrength()) / mass;

            if (velocity.magnitude > neededBulletPenetrationForce)
            {
                CreateDecal(bothHit.outHit);

                transform.position = bothHit.outHit.point;

                // Stopping force of the surface.
                velocity += -velocity.normalized * neededBulletPenetrationForce;

                // Deflection of the bullet from the motion vector.
                float angle = (180 - Vector3.Angle(velocity, bothHit.inHit.normal)) / 90f;
                float deflectionForce = angle * neededBulletPenetrationForce;

                if (deflectionForce < velocity.magnitude)
                {
                    velocity += Random.insideUnitSphere * deflectionForce;
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
            Push();
        }
    }

    private void Ricochet(RaycastBothHit bothHit)
    {
        transform.position = bothHit.inHit.point;
        velocity = Vector3.Reflect(velocity, bothHit.inHit.normal);
    }

    private void CreateDecal(RaycastHit hitInfo)
    {
        PoolManager.Instance.CreateOrPop<Decal>(decalTemplate, hitInfo.point, Quaternion.LookRotation(-hitInfo.normal, Vector3.up));
    }

    private Color GetVelocityColor()
    {
        float a = velocity.magnitude / 500f;
        return gradient.Evaluate(Mathf.Min(a, 1));
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