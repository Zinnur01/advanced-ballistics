using ApexInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HideScriptField]
[RequireComponent(typeof(TrailRenderer))]
public class TracerDisposer : MonoBehaviour
{
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        GetComponent<PoolObject>().OnBeforePushCallback += OnBeforePushCallback;
    }

    private void OnBeforePushCallback()
    {
        trailRenderer.Clear();
    }
}
