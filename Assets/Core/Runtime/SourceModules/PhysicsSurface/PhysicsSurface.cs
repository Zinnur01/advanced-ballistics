using ApexInspector;
using UnityEngine;

[HideScriptField]
public class PhysicsSurface : MonoBehaviour
{
    [SerializeField]
    private PhysicsSurfaceAsset surface;

    #region [Getter / Setter]
    public PhysicsSurfaceAsset GetSurface()
    {
        return surface;
    }
    #endregion
}