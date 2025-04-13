using UnityEngine;
using Fusion;
using System;

public class WeaponPickup : NetworkBehaviour
{
    public WeaponBase weaponData;
    public Action OnItemPicked;

    private bool isPicked = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPicked) return;

        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        PlayerNetworkProperties player = other.GetComponent<PlayerNetworkProperties>();

        if (playerInventory == null || player == null)
        {
            Debug.LogError("Không tìm thấy PlayerInventory hoặc PlayerNetworkProperties!");
            return;
        }

        if (!player.HasInputAuthority) return;

        if (weaponData == null)
        {
            Debug.LogError("WeaponBase chưa được gán vào WeaponPickup!");
            return;
        }

        isPicked = true;

        Debug.Log($"Nhặt vũ khí: {weaponData.name}");
        playerInventory.PickUpWeapon(weaponData);

        OnItemPicked?.Invoke();

        if (Object.HasStateAuthority)
        {
            Runner.Despawn(Object);
        }
    }
}
