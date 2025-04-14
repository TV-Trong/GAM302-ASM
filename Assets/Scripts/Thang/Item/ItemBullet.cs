using UnityEngine;

[CreateAssetMenu(fileName = "NewItemBullet", menuName = "Item System/Bullet Item")]
public class ItemBullet : ItemBase
{
    public WeaponBase[] weaponTypes;
    public int[] ammoAmounts;

    public override void UseItem(PlayerNetworkProperties player, PlayerInventory inventory)
    {
        int count = Mathf.Min(weaponTypes.Length, ammoAmounts.Length);

        for (int i = 0; i < count; i++)
        {
            var weapon = weaponTypes[i];
            var amount = ammoAmounts[i];

            if (weapon != null && inventory.ammoCount.ContainsKey(weapon))
            {
                inventory.ammoCount[weapon] += amount;
                Debug.Log($"Cộng dồn đạn cho {weapon.name}: {amount} đạn");
            }
            else
            {
                Debug.Log($"Không có vũ khí {weapon?.name ?? "Không xác định"} trong kho.");
            }
        }
    }
}
