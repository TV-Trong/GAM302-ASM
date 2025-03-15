using UnityEngine;

[CreateAssetMenu(fileName = "NewShotgun", menuName = "Weapon System/Shotgun")]
public class ShotgunWeapon : WeaponBase
{
    [SerializeField] private int pelletCount = 5;  // Số viên đạn mỗi lần bắn
    [SerializeField] private float spreadAngle = 30f; // Tổng độ xòe (VD: 30 độ)

    public override void Fire(Vector2 position, Vector2 direction)
    {
        float halfSpread = spreadAngle / 2; // Phạm vi lệch sang trái/phải

        for (int i = 0; i < pelletCount; i++)
        {
            // Tạo góc lệch ngẫu nhiên trong phạm vi (-spreadAngle/2) đến (+spreadAngle/2)
            float randomAngle = Random.Range(-halfSpread, halfSpread);

            // Tính toán vector hướng bắn mới bằng cách xoay hướng gốc
            float radianAngle = randomAngle * Mathf.Deg2Rad; // Chuyển đổi sang radian
            Vector2 spreadDirection = new Vector2(
                direction.x * Mathf.Cos(radianAngle) - direction.y * Mathf.Sin(radianAngle),
                direction.x * Mathf.Sin(radianAngle) + direction.y * Mathf.Cos(radianAngle)
            ).normalized; // Chuẩn hóa vector để giữ nguyên tốc độ đạn

            // Tạo viên đạn
            GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = spreadDirection * bulletForce;

            // Debug đường đạn (Chỉ hiển thị trong Scene View)
            Debug.DrawRay(position, spreadDirection * bulletForce, Color.red, 0.5f);
        }
    }
}
