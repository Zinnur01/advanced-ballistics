using ApexInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

public class PhysicsBullet : PoolObject
{
    [SerializeField]
    [Suffix("m/s")]
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
        lastPosition = _transform.position;

        velocity += Physics.gravity * Time.fixedDeltaTime;

        ExternalForcesManager.Impact(ref velocity);

        _transform.Translate(velocity * Time.fixedDeltaTime, Space.World);

        if (PhysicsExtension.LinecastBoth(lastPosition, _transform.position, out RaycastBothHit bothHitInfo))
        {
            CreateDecal(bothHitInfo.inHit);

            _transform.position = bothHitInfo.inHit.point;
            Debug.DrawLine(lastPosition, _transform.position, GetVelocityColor(), 10);

            if (!Through(bothHitInfo))
            {
                Ricochet(bothHitInfo);
            }
        }
        else
        {
            Debug.DrawLine(lastPosition, _transform.position, GetVelocityColor(), 10);
        }
    }

    private bool Through(RaycastBothHit bothHit)
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
                    velocity += Random.insideUnitSphere * (deflectionForce / 10f);
                }
                else
                {
                    Push();
                }
            }
            else
            {
                return false;
                //Push();
            }
        }
        else
        {
            Push();
        }

        return true;
    }

    private void Ricochet(RaycastBothHit bothHit)
    {
        _transform.position = bothHit.inHit.point;
        velocity = Vector3.Reflect(velocity, bothHit.inHit.normal);

        float angle = Vector3.Angle(velocity, bothHit.inHit.normal);
        if (angle < 30f)
        {
            Push();
        }
        else
        {
            velocity *= (angle / 90f) / 5f;
        }
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

public interface IExternalForce
{
    void Impact(ref Vector3 velocity);
}

public static class ExternalForcesManager
{
    private static List<IExternalForce> forces;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void Initialize()
    {
        forces = new List<IExternalForce>();

        var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IExternalForce)));
        foreach (Type type in types)
        {
            forces.Add((IExternalForce)Activator.CreateInstance(type));
        }
    }

    public static void Impact(ref Vector3 velocity)
    {
        for (int i = 0; i < forces.Count; i++)
        {
            forces[i].Impact(ref velocity);
        }
    }
}

public class Wind : IExternalForce
{
    private Vector3 force = new Vector3(0, 0, 10);

    public void Impact(ref Vector3 velocity)
    {
        velocity += force * Time.fixedDeltaTime;
    }
}

public class AirResistance : IExternalForce
{
    private float k = 0.1f;

    public void Impact(ref Vector3 velocity)
    {
        velocity += -velocity * Time.fixedDeltaTime * k;
    }
}