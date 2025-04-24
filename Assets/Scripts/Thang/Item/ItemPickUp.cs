using UnityEngine;
using Fusion;
using System;

public class ItemPickup : NetworkBehaviour
{
    public Action OnItemPicked;

    [SerializeField] private ItemBase itemData;

    [Networked] private bool isPicked { get; set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerNetworkProperties>();
        var inventory = other.GetComponent<PlayerInventory>();

        if (player == null || inventory == null || itemData == null)
            return;

        if (Object.HasStateAuthority)
        {
            // ✅ Host xử lý trực tiếp
            TryPickUp(player, inventory);
        }
        else if (player.HasInputAuthority)
        {
            // ✅ Client gửi yêu cầu lên host
            RpcRequestPickup(player.Object.InputAuthority);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RpcRequestPickup(PlayerRef requestingPlayer)
    {
        var playerObj = Runner.GetPlayerObject(requestingPlayer);
        if (playerObj == null) return;

        var player = playerObj.GetComponent<PlayerNetworkProperties>();
        var inventory = playerObj.GetComponent<PlayerInventory>();

        if (player != null && inventory != null)
        {
            TryPickUp(player, inventory);
        }
    }

    private void TryPickUp(PlayerNetworkProperties player, PlayerInventory inventory)
    {
        if (isPicked) return;

        if (itemData is ItemHP && player.CurrentHP >= player.BaseHP)
        {
            Debug.Log("HP đầy, không thể nhặt HP Item.");
            return;
        }

        isPicked = true;

        itemData.UseItem(player, inventory);
        OnItemPicked?.Invoke();

        Debug.Log($"[Fusion] {player.name} đã nhặt item: {itemData.name}");
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
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        foreach (var collider in GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }
    }
}
