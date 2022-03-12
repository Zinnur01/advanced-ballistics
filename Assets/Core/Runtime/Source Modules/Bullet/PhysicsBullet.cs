using ApexInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PhysicsBullet : PoolObject
{
    [SerializeField]
    [Suffix("Ï/Ò")]
    private int initialSpeed = 715;

    [SerializeField]
    [Suffix("Í„Ò*Ò")]
    private float impulse = 1.25f;

    [SerializeField]
    private float mass = 0.008f;

    [SerializeField]
    private float airResistance = 0.1f;

    [SerializeField]
    private PoolObject decalTemplate;

    // Stored required properties.
    [SerializeField]
    [ReadOnly]
    private Vector3 velocity;

    private Vector3 lastPosition;

    private void OnEnable()
    {
        lastPosition = transform.position;
    }


    private void FixedUpdate()
    {
        //Vector3 reflection = Vector3.Reflect(velocity, -Vector3.ProjectOnPlane(velocity, Vector3.up).normalized);
        //Debug.DrawRay(transform.position, reflection * airResistance);
        //velocity += reflection * airResistance * Time.fixedDeltaTime;

        velocity += Physics.gravity * Time.fixedDeltaTime;

        if (Physics.Linecast(lastPosition, transform.position, out RaycastHit hitInfo))
        {
            PoolManager.Instance.CreateOrPop<Decal>(decalTemplate, hitInfo.point, Quaternion.LookRotation(-hitInfo.normal, Vector3.up));
            transform.position = hitInfo.point;
            velocity = Vector3.Reflect(velocity, hitInfo.normal);
            //Push();
        }

        transform.Translate(velocity * Time.fixedDeltaTime, Space.World);

        Debug.DrawLine(lastPosition, transform.position, Color.red, 5f);

        lastPosition = transform.position;
    }

    protected override void OnBeforePush()
    {
        lastPosition = Vector3.zero;
    }

    public void ApplyVelocity(Vector3 vector)
    {
        velocity = vector * initialSpeed;
    }
}
