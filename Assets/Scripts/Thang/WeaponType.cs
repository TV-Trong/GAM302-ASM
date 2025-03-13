using UnityEngine;

public enum WeaponType { Pistol, Rifle, Shotgun, Grenade }

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon System/Weapon")]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Info")]
    public string weaponName;
    public WeaponType type;
    [SerializeField] Sprite weaponDisplay;

    [Header("Stats")]
    [Range(1f, 100f)] public float damage;
    [Range(0.1f, 5f)] public float fireRate;
    [Range(1, 100)] public int ammoCapacity;
    [Range(0.1f, 5f)] public float reloadTime;
    [Range(0.1f, 100f)] public float bulletForce;

    [Header("Bullet & Sound")]
    public GameObject bulletPrefab;
    public AudioClip fireSound;
    public AudioClip reloadSound;

    [Header("Explosive Properties")]
    public bool isExplosive;
    public ExplosionData explosionData;
}

[System.Serializable]
public struct ExplosionData
{
    [Range(0.1f, 10f)] public float radius;
    [Range(0f, 5f)] public float delay;
}
