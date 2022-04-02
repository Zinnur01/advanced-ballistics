using ApexInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Physics Surface", menuName = "Physics Surface")]
[HideScriptField]
public class PhysicsSurfaceAsset : ScriptableObject
{
    [SerializeField]
    [Suffix("HB")]
    private float strength;

    public float GetStrength()
    {
        return strength;
    }
}