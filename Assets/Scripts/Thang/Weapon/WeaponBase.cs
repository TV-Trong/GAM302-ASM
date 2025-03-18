using UnityEngine;

public abstract class WeaponBase : ScriptableObject
{
    [Header("Weapon Info")]
    [SerializeField] private string weaponName;
    [SerializeField] private WeaponType type;
    [SerializeField] public Sprite weaponDisplay;

    [Header("Stats")]
    [Range(1f, 100f)] [SerializeField] private float damage;
    [Range(0.1f, 5f)] [SerializeField] public float fireRate;
    [Range(1, 100)] [SerializeField] private int ammoCapacity;
    [Range(0.1f, 5f)] [SerializeField] private float reloadTime;
    [Range(0.1f, 100f)] public float bulletForce;

    [Header("Shotgun Settings")]
    [SerializeField] public bool isShotgun = false; // Kiem tra shotgun
    [Range(1, 20)][SerializeField] protected int pelletCount = 5; // Số viên đạn 
    [Range(1f, 45f)][SerializeField] protected float spreadAngle = 10f; // Góc tỏa 

    [Header("Bullet")]
    public GameObject bulletPrefab;

    public abstract void Fire(Vector2 position, Vector2 direction);

    public virtual int GetPelletCount() => pelletCount;
    public virtual float GetSpreadAngle() => spreadAngle;
}
