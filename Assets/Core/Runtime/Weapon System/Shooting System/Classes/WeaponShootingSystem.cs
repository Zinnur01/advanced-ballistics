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
    private Transform cameraTransform;
    private AudioSource audioSource;

    // Stored required properties.
    private float fireDelay;
    private float lastShootTime;

    private void Awake()
    {
        cameraTransform = GetComponentInParent<Camera>().transform;
        audioSource = GetComponent<AudioSource>();
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
            Shoot(cameraTransform.position, cameraTransform.forward);

            if (audioSource != null && shootClip != null)
            {
                audioSource.PlayOneShot(shootClip);
            }
        }
    }

    protected abstract void Shoot(Vector3 origin, Vector3 direction);

    private float RPM2Delay(float rpm)
    {
        return 60f / rpm;
    }
}
