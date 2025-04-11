using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerInventory : NetworkBehaviour
{
    public WeaponBase[] WeaponSlot = new WeaponBase[2];
    public Dictionary<WeaponBase, int> ammoCount = new();
    private WeaponBase currentWeapon;
    private Weapon weaponScript;


    void Start()
    {
        Debug.Log("Bắt đầu kiểm tra vũ khí trong Inventory");

        if (WeaponSlot[0] == null) Debug.LogWarning("WeaponSlot[0] đang rỗng!");
        if (WeaponSlot[1] == null) Debug.LogWarning("WeaponSlot[1] đang rỗng!");
    
        weaponScript =GetComponent<Weapon>();

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

        StartCoroutine(EquipWeapon(WeaponSlot[0]));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Nhấn phím 1 - Đổi sang vũ khí slot 1");
            StartCoroutine(EquipWeapon(WeaponSlot[0]));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Nhấn phím 2 - Đổi sang vũ khí slot 2");
            StartCoroutine(EquipWeapon(WeaponSlot[1]));
        }
    }
    public void PickUpWeapon(WeaponBase newWeapon)
    {
        if (newWeapon == null) return;

        DropWeapon();

        WeaponSlot[1] = newWeapon;

        if (!newWeapon.isInfiniteAmmo && !ammoCount.ContainsKey(newWeapon))
        {
            ammoCount[newWeapon] = newWeapon.maxAmmo;
            Debug.Log($"Nhặt {newWeapon.name}, số đạn: {ammoCount[newWeapon]}");
        }

        StartCoroutine(EquipWeapon(newWeapon)); 
    }

    public void UseAmmo(WeaponBase weapon)
    {
        if (weapon == null) return;

        if (weapon == WeaponSlot[0])
        {
            //Debug.Log("Súng chính không giới hạn đạn.");
            return;
        }

        if (weapon == WeaponSlot[1] && !weapon.isInfiniteAmmo)
        {
            if (ammoCount.ContainsKey(weapon))
            {
                StartCoroutine(RemoveWeapon(weapon));
            }
        }
    }

    IEnumerator RemoveWeapon(WeaponBase weapon)
    {
        ammoCount[weapon]--;
        Debug.Log($"Đạn còn lại: {ammoCount[weapon]}");

        if (weaponScript.weaponData.name == "PumpShotgun")
        {
            yield return new WaitForSeconds(2);
        }
        else
            yield return null;

        if (ammoCount[weapon] <= 0)
        {
            DropWeapon();
            StartCoroutine(EquipWeapon(WeaponSlot[0]));
        }
    }

    IEnumerator EquipWeapon(WeaponBase weapon)
    {
        if (weapon == null)
        {
            yield break;
        }

        if (HasInputAuthority)
            AudioManager.Instance.PlayAudioRpc("Pickup" + weapon.name, "Master/SFX/Gun Shot", transform.root.position);

        weaponScript.isPickingWeapon = true;

        if (weaponScript != null)
        {
            Debug.Log($"EquipWeapon: Đang trang bị {weapon.name}");
            weaponScript.CancelShooting();
            weaponScript.SetWeapon(weapon);
            weaponScript.SetWeaponActive(true); // Đảm bảo vũ khí hiển thị 
        }
        else
        {
            Debug.LogWarning("weaponScript bị null!");
        }

        yield return new WaitForSeconds(2);

        weaponScript.isPickingWeapon = false;
        currentWeapon = weapon;
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
