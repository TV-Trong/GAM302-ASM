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
    public int pelletCount = 5;  // Định nghĩa mặc định cho shotgun
    public float spreadAngle = 10f; // Định nghĩa mặc định cho shotgun

    public GameObject bulletPrefab;

    // Phương thức để bắn, có thể để trống hoặc có logic mặc định
    public virtual void Fire(Vector2 position, Vector2 direction) { }

    public virtual void Reload() => currentAmmo = maxAmmo;

    public virtual int GetPelletCount() => pelletCount; // Trả về số viên đạn shotgun
    public virtual float GetSpreadAngle() => spreadAngle; // Trả về góc tỏa ra của shotgun
}
