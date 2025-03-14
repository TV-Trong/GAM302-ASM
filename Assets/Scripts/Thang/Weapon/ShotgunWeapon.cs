using UnityEngine;

[CreateAssetMenu(fileName = "NewShotgun", menuName = "Weapon System/Shotgun")]
public class ShotgunWeapon : WeaponBase
{
    [SerializeField] private int pelletCount = 5; // Số viên đạn bắn ra cùng lúc
    [SerializeField] private float spreadAngle = 10f; // Độ xòe của đạn

    public override void Fire(Vector2 position, Vector2 direction)
    {
        for (int i = 0; i < pelletCount; i++)
        {
            float angle = Random.Range(-spreadAngle, spreadAngle);
            Vector2 spreadDirection = Quaternion.Euler(0, 0, angle) * direction;

            GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = spreadDirection * bulletForce;
        }
    }
}
