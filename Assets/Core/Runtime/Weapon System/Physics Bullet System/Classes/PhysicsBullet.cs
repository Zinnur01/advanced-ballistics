using ApexInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBullet : PoolObject
{
    [SerializeField]
    [Suffix("ì/ñ")]
    private int initialSpeed = 710;

    [SerializeField]
    private float mass = 0.0067f;

    [SerializeField]
    private PoolObject decalTemplate;

    [SerializeField]
    private LayerMask cullingLayer;

    [SerializeField]
    private float k;

    // Stored required components.
    private Transform _transform;

    // Stored required properties.
    private Vector3 lastPosition;
    private Vector3 velocity;

    protected override void Awake()
    {
        base.Awake();
        _transform = transform;
    }

    public void Initialize(Vector3 vector)
    {
        velocity = vector * initialSpeed;
    }

    private void FixedUpdate()
    {
        Debug.Log(_transform.position.y);
        lastPosition = _transform.position;

        velocity += -velocity * Time.fixedDeltaTime * k;
        velocity += Physics.gravity * Time.fixedDeltaTime;

        _transform.Translate(velocity * Time.fixedDeltaTime, Space.World);

        if (PhysicsExtension.LinecastBoth(lastPosition, _transform.position, out RaycastBothHit bothHitInfo))
        {
            CreateDecal(bothHitInfo.inHit);

            _transform.position = bothHitInfo.inHit.point;
            Debug.DrawLine(lastPosition, _transform.position, GetVelocityColor(), 10);

            Through(bothHitInfo);
        }
        else
        {
            Debug.DrawLine(lastPosition, _transform.position, GetVelocityColor(), 10);
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

                _transform.position = bothHit.outHit.point;

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
        _transform.position = bothHit.inHit.point;
        velocity = Vector3.Reflect(velocity, bothHit.inHit.normal);
    }

    private void CreateDecal(RaycastHit hitInfo)
    {
        PoolManager.Instance.CreateOrPop<Decal>(decalTemplate, hitInfo.point, Quaternion.LookRotation(-hitInfo.normal, Vector3.up));
    }

    private Color GetVelocityColor()
    {
        float percent = velocity.magnitude / initialSpeed;
        if (percent > 0.75f)
        {
            return Color.red;
        }
        else if (percent > 0.5f)
        {
            return Color.yellow;
        }
        else if (percent > 0.25f)
        {
            return Color.green;
        }
        else if (percent > 0.1f)
        {
            return Color.blue;
        }
        else
        {
            return Color.white;
        }
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