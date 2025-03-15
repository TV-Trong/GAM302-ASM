using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponBase weapon; // Tham chiếu đến ScriptableObject vũ khí

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Chỉ cho phép Player nhặt
        {
            Debug.Log("Nhặt được vũ khí: " + weapon.name);
            Destroy(gameObject); // Xóa GameObject vũ khí sau khi nhặt
        }
    }
}
