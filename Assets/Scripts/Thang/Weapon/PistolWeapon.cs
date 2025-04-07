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

        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = direction * bulletForce;
        }
        else
        {
            Debug.LogError("Rigidbody2D component not found on bullet prefab!");
        }
    }
}
