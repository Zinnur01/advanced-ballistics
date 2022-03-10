using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShootingSystem : MonoBehaviour
{
    [SerializeField]
    private int rpm = 300;

    [SerializeField]
    private Transform firePoint;

    // Stored required properties.
    private float fireDelay;
    private float lastShootTime;

    private void Start()
    {
        fireDelay = RPM2Delay(rpm);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && lastShootTime + fireDelay < Time.time)
        {
            lastShootTime = Time.time;
            Shoot();
            Debug.DrawRay(firePoint.position, firePoint.forward * float.MaxValue, Color.red, 10);
        }
    }

    protected virtual void Shoot()
    {

    }

    private float RPM2Delay(float rpm)
    {
        return 60f / rpm;
    }
}
