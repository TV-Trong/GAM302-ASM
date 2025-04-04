using UnityEngine;

public abstract class ItemBase : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;

    public abstract void UseItem(PlayerNetworkProperties player, PlayerInventory inventory);
}
