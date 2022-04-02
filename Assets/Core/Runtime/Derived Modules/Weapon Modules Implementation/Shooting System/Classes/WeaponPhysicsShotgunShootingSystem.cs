using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ApexInspector;

public class WeaponPhysicsShotgunShootingSystem : WeaponPhysicsShootingSystem
{
    [SerializeField]
    private int ballCount = 7;

    [SerializeField]
    [MinMaxSlider(0, 1)]
    private float deflection = 0.3f;

    protected override void Shoot(Vector3 origin, Vector3 direction)
    {
        for (int i = 0; i < ballCount; i++)
        {
            base.Shoot(origin, RandomizeDirection(direction));
        }
    }

    private Vector3 RandomizeDirection(Vector3 direction)
    {
        return (direction + Random.insideUnitSphere * deflection).normalized;
    }
}
