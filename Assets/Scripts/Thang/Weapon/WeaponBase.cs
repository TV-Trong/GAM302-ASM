using UnityEngine;

public abstract class WeaponBase : ScriptableObject
{
    [Header("Weapon Info")]
    [SerializeField] private string weaponName;
    [SerializeField] private WeaponType type;
    [SerializeField] protected Sprite weaponDisplay;

    [Header("Stats")]
    [Range(1f, 100f)] public float damage;
    [Range(0.1f, 5f)] public float fireRate;
    [Range(1, 100)] public int ammoCapacity;
    [Range(0.1f, 5f)] public float reloadTime;
    [Range(0.1f, 100f)] public float bulletForce;

    [Header("Bullet")]
    public GameObject bulletPrefab;

    public abstract void Fire(Vector2 position, Vector2 direction);
}
