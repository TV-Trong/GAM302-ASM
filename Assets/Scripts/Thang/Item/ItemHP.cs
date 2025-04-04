using UnityEngine;

[CreateAssetMenu(fileName = "NewItemHP", menuName = "Item System/HP Item")]
public class ItemHP : ItemBase
{
    public float healAmount;

    public override void UseItem(PlayerNetworkProperties player, PlayerInventory inventory)
    {
        if (player.CurrentHP < player.BaseHP)
        {
            player.HealRpc(healAmount);
            Debug.Log($"Hồi máu: {healAmount}");
        }
        else
        {
            Debug.Log("HP đầy, không cần hồi máu.");
        }
    }
}
