using UnityEngine;

public class LaserShootingSystem : MonoBehaviour
{
    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = GetComponentInParent<Camera>().transform;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hitInfo))
            {
                if (hitInfo.transform.TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    damageable.Damage(hitInfo.point, 2f);
                }
            }
        }
    }
}
