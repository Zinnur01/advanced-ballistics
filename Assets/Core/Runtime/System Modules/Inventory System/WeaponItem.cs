using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class WeaponItem : ScriptableObject
{
    [SerializeField]
    private WeaponIdentifier weaponIdentifier;

    #region [Getter / Setter]
    public WeaponIdentifier GetWeaponIdentifier()
    {
        return weaponIdentifier;
    }
    #endregion
}
