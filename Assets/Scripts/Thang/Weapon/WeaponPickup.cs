using UnityEngine;
using System;

public class WeaponPickup : MonoBehaviour
{
    public WeaponBase weaponData;

    public Action OnItemPicked;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory == null)
        {
            Debug.LogError("Player không có PlayerInventory!");
            return;
        }

        if (weaponData == null)
        {
            Debug.LogError("WeaponBase chưa được gán vào WeaponPickup!");
            return;
        }

        Debug.Log($"Nhặt vũ khí: {weaponData.name}");

        playerInventory.PickUpWeapon(weaponData);

        Debug.Log("Vật phẩm đã được nhặt. Gọi sự kiện spawn lại.");
        OnItemPicked?.Invoke();
        Destroy(gameObject);
    }
}
