using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField]
    private Transform weaponHinge;

    [SerializeField]
    private List<WeaponItem> items = new List<WeaponItem>();

    private WeaponItem equippedWeapons;

    private void Awake()
    {
        if (items.Count > 0)
        {
            Equip(items.First());
        }
    }

    private void Equip(WeaponItem item)
    {
        RemoveChildren(weaponHinge);
        equippedWeapons = item;
        Instantiate(item.GetWeaponIdentifier(), weaponHinge);
    }

    private void RemoveChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child);
        }
    }
}