using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PoolObject))]
public class AutoPush : MonoBehaviour
{
    [SerializeField]
    [Min(0f)]
    private float delay = 1f;

    // Stored required components.
    private PoolObject poolObject;

    // Stored required properties.
    private CoroutineObject timerCoroutine;

    private void Awake()
    {
        poolObject = GetComponent<PoolObject>();
        timerCoroutine = new CoroutineObject(this);
    }

    private void OnEnable()
    {
        timerCoroutine.Start(Timer, true);
    }

    private void OnDisable()
    {
        timerCoroutine.Stop();
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(delay);
        poolObject.Push();
    }
}
