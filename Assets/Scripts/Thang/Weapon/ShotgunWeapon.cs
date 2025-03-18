using UnityEngine;

[CreateAssetMenu(fileName = "NewShotgun", menuName = "Weapon System/Shotgun")]
public class ShotgunWeapon : WeaponBase
{
    [SerializeField] private int shotgunPelletCount ;  // Shotgun bắn 8 viên
    [SerializeField] private float shotgunSpreadAngle ; // Tỏa ra 30 độ

    public override void Fire(Vector2 position, Vector2 direction)
    {
        float halfSpread = shotgunSpreadAngle / 2;

        for (int i = 0; i < shotgunPelletCount; i++)
        {
            float randomAngle = Random.Range(-halfSpread, halfSpread);
            Vector2 spreadDirection = Quaternion.Euler(0, 0, randomAngle) * direction;

            GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = spreadDirection * bulletForce;

            Debug.DrawRay(position, spreadDirection * bulletForce, Color.red, 0.5f);
        }
    }

    public override int GetPelletCount() => shotgunPelletCount;
    public override float GetSpreadAngle() => shotgunSpreadAngle;
}
