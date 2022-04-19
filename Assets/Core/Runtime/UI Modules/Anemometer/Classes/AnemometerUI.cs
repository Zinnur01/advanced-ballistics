using Runtime.SourceModules.ExternalForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnemometerUI : MonoBehaviour
{
    [SerializeField]
    private Wind windAsset;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private Transform propeller;

    private void Update()
    {
        Vector3 project = Vector3.Project(windAsset.GetVelocity(), player.forward);
        float force = project.magnitude;
        float dot = Mathf.Sign(Vector3.Dot(player.forward, windAsset.GetVelocity()));

        propeller.Rotate(new Vector3(0, 0, force * Time.deltaTime * dot));
    }
}
