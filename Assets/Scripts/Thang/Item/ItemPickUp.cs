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
            if (itemData is ItemHP itemHP && player.CurrentHP >= player.BaseHP)
            {
                Debug.Log("HP đầy, không thể nhặt HP Item.");
                return;
            }

            if (itemData is ItemBullet itemBullet && inventory.ammoCount.ContainsKey(itemBullet.weaponType))
            {
                inventory.ammoCount[itemBullet.weaponType] += itemBullet.ammoAmount;
            }
            else if (itemData is ItemBullet)
            {
                Debug.Log("Vũ khí không có trong kho đồ, không thể cộng đạn.");
            }

            itemData.UseItem(player, inventory);

            Debug.Log("Vật phẩm đã được nhặt. Gọi sự kiện spawn lại.");
            OnItemPicked?.Invoke();
            Destroy(gameObject);
        }
    }
}
