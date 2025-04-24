using UnityEngine;
using Fusion;
using System;

[RequireComponent(typeof(NetworkTransform))]
public class WeaponPickup : NetworkBehaviour
{
    public WeaponBase weaponData;
    public Action OnItemPicked;

    [Networked] private bool isPicked { get; set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var playerInventory = other.GetComponent<PlayerInventory>();
        var player = other.GetComponent<PlayerNetworkProperties>();

        if (playerInventory == null || player == null) return;
        if (weaponData == null) return;

        if (Object.HasStateAuthority)
        {
            TryPickUp(playerInventory, player);
        }
        else if (player.HasInputAuthority)
        {
            // Gửi yêu cầu pickup thông qua player
            playerInventory.RequestPickupWeapon(Object); // 👈 Gọi qua Player
        }
    }

    public void TryPickUp(PlayerInventory inventory, PlayerNetworkProperties player)
    {
        if (isPicked) return;

        isPicked = true;

        inventory.PickUpWeapon(weaponData);
        OnItemPicked?.Invoke();

        Debug.Log($"[Fusion] {player.name} đã nhặt {weaponData.name}");
    }

    public override void FixedUpdateNetwork()
    {
        if (isPicked)
        {
            HideVisual();

            if (Object.HasStateAuthority)
            {
                Runner.Despawn(Object);
            }
        }
    }

    private void HideVisual()
    {
        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = false;

        foreach (var c in GetComponentsInChildren<Collider2D>())
            c.enabled = false;
    }
}
