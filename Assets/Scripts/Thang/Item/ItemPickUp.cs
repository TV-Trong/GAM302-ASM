using UnityEngine;
using Fusion;
using System;

public class ItemPickup : NetworkBehaviour
{
    public Action OnItemPicked;

    [SerializeField] private ItemBase itemData;
    private bool isPicked = false; // Tránh trigger nhiều lần

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPicked) return;

        PlayerNetworkProperties player = other.GetComponent<PlayerNetworkProperties>();
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (player != null && inventory != null && itemData != null)
        {
            // ✅ Chỉ cho phép player có quyền xử lý (InputAuthority hoặc StateAuthority)
            if (!player.HasInputAuthority) return;

            // Kiểm tra nếu item là hồi máu và máu đã đầy thì không cho nhặt
            if (itemData is ItemHP && player.CurrentHP >= player.BaseHP)
            {
                Debug.Log("HP đầy, không thể nhặt HP Item.");
                return;
            }

            isPicked = true;

            itemData.UseItem(player, inventory);
            Debug.Log("Vật phẩm đã được nhặt. Gọi sự kiện spawn lại.");
            OnItemPicked?.Invoke();

            // ✅ Chỉ người có quyền mới yêu cầu despawn
            if (Object.HasStateAuthority)
            {
                Runner.Despawn(Object);
            }
        }
    }
}
