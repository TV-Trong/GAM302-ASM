using UnityEngine;

[CreateAssetMenu(fileName = "NewPistol", menuName = "Weapon System/Pistol")]
public class PistolWeapon : WeaponBase
{
    public override void Fire(Vector2 position, Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletForce;
    }
}
