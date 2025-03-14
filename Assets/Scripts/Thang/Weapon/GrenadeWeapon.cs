using UnityEngine;

[CreateAssetMenu(fileName = "NewGrenade", menuName = "Weapon System/Grenade")]
public class GrenadeWeapon : WeaponBase
{
    [Header("Explosive Properties")]
    public ExplosionData explosionData;

    public override void Fire(Vector2 position, Vector2 direction)
    {
        GameObject grenade = Instantiate(bulletPrefab, position, Quaternion.identity);
        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletForce;

        grenade.GetComponent<Grenade>().Initialize(explosionData);
    }
}
