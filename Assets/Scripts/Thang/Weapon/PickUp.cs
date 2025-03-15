using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Weapon weaponScript; // Kéo script Weapon trên Player vào đây

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Chạm vào: " + other.name); // Kiểm tra đối tượng va chạm

        WeaponPickup weaponPickup = other.GetComponent<WeaponPickup>();

        if (weaponPickup == null)
        {
            Debug.LogError("WeaponPickup trên object bị thiếu!");
            return;
        }

        if (weaponPickup.weapon == null)
        {
            Debug.LogError("Vũ khí trong WeaponPickup chưa được gán!");
            return;
        }

        if (weaponScript == null)
        {
            Debug.LogError("weaponScript trên Player chưa được thiết lập!");
            return;
        }

        Debug.Log("Nhặt vũ khí: " + weaponPickup.weapon.name);
        weaponScript.SetWeapon(weaponPickup.weapon);
        Destroy(other.gameObject);
    }
}
