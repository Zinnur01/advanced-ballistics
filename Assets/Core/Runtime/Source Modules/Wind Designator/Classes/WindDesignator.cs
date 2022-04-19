using Runtime.SourceModules.ExternalForce;
using UnityEngine;

public class WindDesignator : MonoBehaviour
{
    [SerializeField]
    private Transform head;

    [SerializeField]
    private Wind wind;

    private void Awake()
    {
        UpdateRotation(wind.GetVelocity());
    }

    private void OnEnable()
    {
        wind.OnChangeVelocity += UpdateRotation;
    }

    private void OnDisable()
    {
        wind.OnChangeVelocity -= UpdateRotation;
    }

    private void UpdateRotation(Vector3 velocity)
    {
        head.rotation = Quaternion.LookRotation(velocity);
    }
}
