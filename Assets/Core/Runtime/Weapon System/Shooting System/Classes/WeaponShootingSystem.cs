using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponShootingSystem : MonoBehaviour
{
    [SerializeField]
    protected int rpm = 300;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private AudioClip shootClip;

    // Stored required components.
    private AudioSource audioSource;
    private Transform cameraTransform;

    // Stored required properties.
    private float fireDelay;
    private float lastShootTime;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        cameraTransform = GetComponentInParent<Camera>().transform;
    }

    private void Start()
    {
        fireDelay = RPM2Delay(rpm);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && lastShootTime + fireDelay < Time.time)
        {
            lastShootTime = Time.time;
            Shoot(firePoint.position, GetShootDirection());

            if (audioSource != null && shootClip != null)
            {
                audioSource.PlayOneShot(shootClip);
            }
        }
    }

    protected abstract void Shoot(Vector3 origin, Vector3 direction);

    private Vector3 GetShootDirection()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.distance > 1f)
            {
                return (hitInfo.point - firePoint.position).normalized;
            }
        }
        return firePoint.forward;
    }

    private float RPM2Delay(float rpm)
    {
        return 60f / rpm;
    }
}
