using UnityEngine;

[CreateAssetMenu(fileName = "NewPistol", menuName = "Weapon System/Pistol")]
public class PistolWeapon : WeaponBase
{
    public override void Fire(Vector2 position, Vector2 direction)
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab is not assigned!");
            return;
        }

        // Tạo viên đạn
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);

        // Lấy Rigidbody2D để áp dụng vận tốc
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Đảm bảo viên đạn đi theo hướng chính xác
            rb.velocity = direction * bulletForce;
        }
        else
        {
            Debug.LogError("Rigidbody2D component not found on bullet prefab!");
        }
    }
}
