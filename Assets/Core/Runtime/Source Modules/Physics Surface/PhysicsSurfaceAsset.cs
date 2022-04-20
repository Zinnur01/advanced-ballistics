using ApexInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Physics Surface", menuName = "Physics Surface")]
[HideScriptField]
public class PhysicsSurfaceAsset : ScriptableObject
{
    [Suffix("HB")]
    public float strength;
}