using UnityEngine;
using System;

public class ItemPickup : MonoBehaviour
{
    public Action OnItemPicked;

    [SerializeField] private ItemBase itemData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerNetworkProperties player = other.GetComponent<PlayerNetworkProperties>();
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (player != null && inventory != null && itemData != null)
        {
            // Kiểm tra nếu item là hồi máu và máu đã đầy thì không cho nhặt
            if (itemData is ItemHP && player.CurrentHP >= player.BaseHP)
            {
                Debug.Log("HP đầy, không thể nhặt HP Item.");
                return;
            }

            // Gọi logic sử dụng item (cộng máu, đạn, v.v.)
            itemData.UseItem(player, inventory);

            Debug.Log("Vật phẩm đã được nhặt. Gọi sự kiện spawn lại.");
            OnItemPicked?.Invoke();
            Destroy(gameObject);
        }
    }
}
