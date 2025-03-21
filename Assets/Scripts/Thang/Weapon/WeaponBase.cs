using UnityEngine;

public abstract class WeaponBase : ScriptableObject
{
    [Header("Weapon Info")]
    [SerializeField] private string weaponName;
    [SerializeField] public Sprite weaponDisplay;

    [Header("Stats")]
    [Range(1f, 100f)] public float damage;
    [Range(0.1f, 5f)] public float fireRate;
    [Range(1, 100)] public int maxAmmo;
    [Range(0.1f, 5f)] public float reloadTime;
    [Range(0.1f, 100f)] public float bulletForce;

    [Header("Ammo Settings")]
    public bool isInfiniteAmmo = false;
    public int ammoCapacity;
    [HideInInspector] public int currentAmmo;

    [Header("Shotgun Settings")]
    public bool isShotgun = false;
    public int pelletCount = 5;
    public float spreadAngle = 10f;

    public GameObject bulletPrefab;

    public abstract void Fire(Vector2 position, Vector2 direction);
    public virtual void Reload() => currentAmmo = maxAmmo;

    public virtual int GetPelletCount() => 1;
    public virtual float GetSpreadAngle() => 0f;

}
