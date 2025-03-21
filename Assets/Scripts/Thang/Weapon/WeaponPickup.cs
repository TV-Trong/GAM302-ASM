using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponBase weaponData; // Dữ liệu vũ khí từ ScriptableObject

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Chạm vào: {other.name}");

        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory == null)
        {
            Debug.LogError("Player không có PlayerInventory!");
            return;
        }

        if (weaponData == null)
        {
            Debug.LogError("WeaponBase chưa được gán vào WeaponPickup!");
            return;
        }

        Debug.Log($"Nhặt vũ khí: {weaponData.name}");

        playerInventory.PickUpWeapon(weaponData);
        Destroy(gameObject);
    }
}
