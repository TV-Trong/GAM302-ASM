using UnityEngine;

[CreateAssetMenu(fileName = "NewShotgun", menuName = "Weapon System/Shotgun")]
public class ShotgunWeapon : WeaponBase
{
    public override void Fire(Vector2 position, Vector2 direction)
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab is not assigned!");
            return;
        }

        // Lấy số lượng viên đạn và góc tỏa từ lớp cha (WeaponBase)
        int pelletCount = GetPelletCount();
        float spreadAngle = GetSpreadAngle();

        for (int i = 0; i < pelletCount; i++)
        {
            // Tính toán góc phân tán của từng viên đạn
            float angleOffset = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);  // Ngẫu nhiên giữa góc phân tán
            Vector2 spreadDirection = Quaternion.Euler(0, 0, angleOffset) * direction;

            // Tạo viên đạn
            GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);

            // Lấy Rigidbody2D và thiết lập vận tốc
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = spreadDirection * bulletForce;
            }
            else
            {
                Debug.LogError("Rigidbody2D component not found on bullet prefab!");
            }
        }
    }
}
