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

    private int equippedIndex;
    private GameObject weaponObject;

    private void Awake()
    {
        if (items.Count > 0)
        {
            Equip(items.First());
        }
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            equippedIndex = (int)Mathf.PingPong(equippedIndex + Mathf.Sign(scroll), items.Count);
            Equip(items[equippedIndex]);
        }
    }

    private void Equip(WeaponItem item)
    {
        if (weaponObject != null)
        {
            Destroy(weaponObject);
        }

        weaponObject = Instantiate(item.GetWeaponIdentifier(), weaponHinge).gameObject;
    }
}