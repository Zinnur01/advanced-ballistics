using ApexInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
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

    // Stored required components.

    // Stored required properties.
    [SerializeField]
    [ReadOnly]
    private Vector3 velocity;

    private Vector3 lastPosition;

    private void FixedUpdate()
    {
        velocity += Physics.gravity * Time.fixedDeltaTime;
        transform.Translate(velocity * Time.fixedDeltaTime, Space.World);

        Debug.DrawLine(lastPosition, transform.position, Color.red, 5f);
        lastPosition = transform.position;
    }

    public void ApplyVelocity(Vector3 vector)
    {
        velocity = vector * initialSpeed;
    }
}
