using UnityEngine;

[CreateAssetMenu(fileName = "NewRifle", menuName = "Weapon System/Rifle")]
public class RifleWeapon : WeaponBase
{
    public override void Fire(Vector2 position, Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletForce;
    }
}
