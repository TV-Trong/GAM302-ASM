using UnityEngine;

public class Gun : MonoBehaviour
{
    public WeaponData weaponData; // Tham chiếu đến ScriptableObject

    private float nextFireTime = 0f;
    private int currentAmmo;

    void Start()
    {
        currentAmmo = weaponData.ammoCapacity; // Gán số đạn ban đầu
    }

    void Update()
    {
        if (weaponData.type != WeaponType.Grenade) // Không áp dụng cho bom
        {
            if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1")) // Bom chỉ ném một lần khi nhấn
            {
                ThrowGrenade();
            }
        }
    }

    void Shoot()
    {
        if (currentAmmo > 0)
        {
            nextFireTime = Time.time + 1f / weaponData.fireRate;
            Instantiate(weaponData.bulletPrefab, transform.position, transform.rotation);
            AudioSource.PlayClipAtPoint(weaponData.fireSound, transform.position);
            currentAmmo--;

            Debug.Log(weaponData.weaponName + " fired! Ammo left: " + currentAmmo);
        }
        else
        {
            Debug.Log("Out of ammo! Reload required.");
        }
    }

    void Reload()
    {
        Debug.Log("Reloading " + weaponData.weaponName);
        Invoke(nameof(FinishReload), weaponData.reloadTime);
    }

    void FinishReload()
    {
        currentAmmo = weaponData.ammoCapacity;
        Debug.Log(weaponData.weaponName + " reloaded.");
    }

    void ThrowGrenade()
    {
        GameObject grenade = Instantiate(weaponData.bulletPrefab, transform.position, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
        Debug.Log("Grenade thrown!");
    }
}
