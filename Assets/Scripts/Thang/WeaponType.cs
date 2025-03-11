using UnityEngine;

// Enum để phân loại vũ khí
public enum WeaponType { Pistol, AK47, Shotgun, Grenade }

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon System/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public WeaponType type;     // Loại ũ khí
    public float damage;        // sát thương
    public float fireRate;      //Tốc độ đạn bắn
    public int ammoCapacity;    //Số lượng đạn tối đa trong 1 băng đạn
    public float reloadTime;    // thời gian nạp đạn
    public float range;         //Khoảng cách tấn công
    public GameObject bulletPrefab; // Đạn hoặc hiệu ứng bắn
    public AudioClip fireSound;
    public AudioClip reloadSound;

    public bool isExplosive;    // xác định vũ khí co phải bom hay không
    public float explosionRadius;   // bán kính vụ nổ
    public float explosionDelay;    // thời gian delay khi nổ

    // Điều chỉnh sát thương dựa trên khoảng cách (chủ yếu cho Shotgun)
    public bool hasDamageFalloff;
    public float minDamage;
    public float maxDamage;
}
