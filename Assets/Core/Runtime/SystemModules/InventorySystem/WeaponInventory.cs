using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField]
    private Transform weaponHinge;

    [SerializeField]
    private List<WeaponItem> items = new List<WeaponItem>();

    private List<WeaponIdentifier> weapons = new List<WeaponIdentifier>();
    private WeaponIdentifier equippedWeapon;
    private int equippedIndex = -1;

    private void Awake()
    {
        InitializeWeapons();
        Equip(0);
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            Equip(equippedIndex + (int)Mathf.Sign(scroll));
        }
    }

    private void InitializeWeapons()
    {
        for (int i = 0; i < items.Count; i++)
        {
            WeaponIdentifier weaponIdentifier = GameObject.Instantiate<WeaponIdentifier>(items[i].GetWeaponIdentifier(), weaponHinge);
            weaponIdentifier.gameObject.SetActive(false);
            weapons.Add(weaponIdentifier);
        }
    }

    private void Equip(int index)
    {
        if (index != equippedIndex && index >= 0 && index < weapons.Count)
        {
            if (equippedWeapon != null)
            {
                equippedWeapon.gameObject.SetActive(false);
            }

            equippedIndex = index;
            equippedWeapon = weapons[index];
            equippedWeapon.gameObject.SetActive(true);
        }
    }
}
