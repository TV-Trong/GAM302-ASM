using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public WeaponBase[] WeaponSlot = new WeaponBase[2];
    public Dictionary<WeaponBase, int> ammoCount = new();

    void Start()
    {
        GameObject pistolObject = GameObject.FindGameObjectWithTag("Pistol");
        if (pistolObject != null)
        {
            Weapon pistolScript = pistolObject.GetComponent<Weapon>();
            if (pistolScript != null)
            {
                WeaponSlot[0] = pistolScript.weaponData;
                if (!pistolScript.weaponData.isInfiniteAmmo)
                {
                    ammoCount[pistolScript.weaponData] = pistolScript.weaponData.maxAmmo;
                }
            }
        }
    }

    public void PickUpWeapon(WeaponBase newWeapon)
    {
        if (newWeapon == null) return;
        DropWeapon();
        WeaponSlot[1] = newWeapon;
        if (!newWeapon.isInfiniteAmmo)
        {
            ammoCount[newWeapon] = newWeapon.maxAmmo;
        }
    }

    public void UseAmmo(WeaponBase weapon)
    {
        if (weapon == null || weapon.isInfiniteAmmo) return;
        if (ammoCount.ContainsKey(weapon))
        {
            ammoCount[weapon]--;
            if (ammoCount[weapon] <= 0) DropWeapon();
        }
    }

    void DropWeapon()
    {
        if (WeaponSlot[1] != null)
        {
            ammoCount.Remove(WeaponSlot[1]);
            WeaponSlot[1] = null;
        }
    }
}
