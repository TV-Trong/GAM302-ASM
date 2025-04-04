using UnityEngine;

[CreateAssetMenu(fileName = "NewItemBullet", menuName = "Item System/Bullet Item")]
public class ItemBullet : ItemBase
{
    public WeaponBase weaponType;
    public int ammoAmount;

    public override void UseItem(PlayerNetworkProperties player, PlayerInventory inventory)
    {
        if (weaponType != null && inventory.ammoCount.ContainsKey(weaponType))
        {
            inventory.ammoCount[weaponType] += ammoAmount;
            Debug.Log($"Cộng dồn đạn cho {weaponType.name}: {ammoAmount} đạn");
        }
        else
        {
            Debug.Log("Không có vũ khí tương ứng để cộng đạn.");
        }
    }
}
