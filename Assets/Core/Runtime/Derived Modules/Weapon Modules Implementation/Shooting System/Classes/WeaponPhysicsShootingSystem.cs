using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPhysicsShootingSystem : WeaponShootingSystem
{
    [SerializeField]
    private PhysicsBullet bulletTemplate;

    protected override void Shoot()
    {
        PhysicsBullet bullet = PoolManager.Instance.CreateOrPop<PhysicsBullet>(bulletTemplate, firePoint.position, firePoint.rotation);
        bullet.Initialize(firePoint.forward);
    }
}
