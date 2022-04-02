using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPhysicsShootingSystem : WeaponShootingSystem
{
    [SerializeField]
    private PhysicsBullet bulletTemplate;

    protected override void Shoot(Vector3 origin, Vector3 direction)
    {
        PhysicsBullet bullet = PoolManager.Instance.CreateOrPop<PhysicsBullet>(bulletTemplate, origin, Quaternion.LookRotation(direction));
        bullet.Initialize(direction);
    }
}
