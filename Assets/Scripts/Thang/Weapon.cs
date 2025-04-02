using System.Collections;
using Fusion;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [SerializeField] public WeaponBase weaponData; 
    [SerializeField] private GameObject bulletTrailPrefab; 
    [SerializeField] private SpriteRenderer weaponSpriteRenderer; 
    [SerializeField] private Transform playerTransform; 
    [SerializeField] private Transform firePoint; 

    [SerializeField] LayerMask ignoredLayer;

    private bool canShoot = false;
    private PlayerInventory inventory;

    private float timer;
    void Start()
    {
        inventory = FindObjectOfType<PlayerInventory>();
        if (weaponData == null)
        {
            Debug.LogWarning("Chưa có vũ khí! Cần nhặt vũ khí trước khi bắn.");
            canShoot = false;
        }
        else
        {
            UpdateWeaponSprite();
            canShoot = true;
            weaponData.currentAmmo = weaponData.maxAmmo; 
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            canShoot = true;
        }

        if (!HasStateAuthority || weaponData == null) 
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (weaponData.isInfiniteAmmo || weaponData.currentAmmo > 0)
            {
                if (weaponData.isShotgun)
                {
                    ShootShotgun();
                }
                else
                {
                    InvokeRepeating("Shoot", 0, weaponData.fireRate);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            CancelShooting();
        }
    }

    public void CancelShooting()
    {
        CancelInvoke("Shoot");
    }

    void Shoot()
    {
        if (weaponData.currentAmmo <= 0 && !weaponData.isInfiniteAmmo)
            return;

        if (!canShoot)
            return;

        timer = weaponData.fireRate;
        canShoot = false;

        inventory.UseAmmo(weaponData);

        AudioManager.Instance.PlayAudioRpc("AK47", "Master/SFX/Gun Shot", transform.root.position);

        Vector2 mouseWorldPosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPosition - (Vector2)playerTransform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, weaponData.bulletForce, ~ignoredLayer);
        Vector2 targetPoint = hit.collider != null ? hit.point : (Vector2)firePoint.position + direction * weaponData.bulletForce;

        NetworkObject bulletTrail = Runner.Spawn(bulletTrailPrefab, firePoint.position, Quaternion.identity);
        StartCoroutine(MoveTrail(bulletTrail, targetPoint));

        if (hit.collider != null)
        {
            HitPlayer(hit);
        }
    }

    void ShootShotgun()
    {
        if (weaponData.currentAmmo <= 0 && !weaponData.isInfiniteAmmo)
            return;

        if (!canShoot)
            return;

        timer = weaponData.fireRate;
        canShoot = false;

        inventory.UseAmmo(weaponData);

        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPosition - (Vector2)playerTransform.position).normalized;

        int pelletCount = weaponData.GetPelletCount();
        float spreadAngle = weaponData.GetSpreadAngle();
        float halfSpread = (pelletCount - 1) / 2f;

        for (int i = 0; i < pelletCount; i++)
        {
            // Tính toán góc phân tán cho viên đạn
            float angleOffset = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);  // Random góc phân tán cho mỗi viên
            Vector2 spreadDirection = Quaternion.Euler(0, 0, angleOffset) * direction;

            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, spreadDirection, weaponData.bulletForce, ~ignoredLayer);
            Vector2 targetPoint = hit.collider != null ? hit.point : (Vector2)firePoint.position + spreadDirection * weaponData.bulletForce;

            // Tạo và di chuyển trail cho viên đạn
            NetworkObject bulletTrail = Runner.Spawn(bulletTrailPrefab, firePoint.position, Quaternion.identity);
            StartCoroutine(MoveTrail(bulletTrail, targetPoint));

            if (hit.collider != null)
            {
                // Kiểm tra nếu viên đạn trúng đối tượng có tag là "Player"
                HitPlayer(hit);
            }
        }
    }




    private void HitPlayer(RaycastHit2D hit)
    {
        if (hit.transform.CompareTag("Player") && hit.transform != transform.root)
        {
            hit.transform.GetComponent<PlayerNetworkProperties>().TakeDamageRpc(weaponData.damage);
        }
    }

    private IEnumerator MoveTrail(NetworkObject trail, Vector2 endPoint)
    {
        float time = 0f;
        float duration = Vector2.Distance(trail.transform.position, endPoint) / weaponData.bulletForce;

        while (time < duration)
        {
            trail.transform.position = Vector2.Lerp(trail.transform.position, endPoint, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        trail.transform.position = endPoint;

        yield return new WaitForSeconds(0.2f);
        DespawnTrail(trail);
    }

    void DespawnTrail(NetworkObject trail)
    {
        Runner.Despawn(trail);
    }

    public void SetWeapon(WeaponBase newWeapon)
    {
        if (newWeapon == null)
        {
            Debug.LogWarning("SetWeapon: Vũ khí mới là null!");
            return;
        }

        weaponData = newWeapon;
        UpdateWeaponSprite();
        canShoot = true;

        Debug.Log($"Trang bị vũ khí: {weaponData.name}");
    }

    public void SetWeaponActive(bool active)
    {
        canShoot = active;
        weaponSpriteRenderer.enabled = active;
    }

    private void UpdateWeaponSprite()
    {
        if (weaponSpriteRenderer != null && weaponData.weaponDisplay != null)
        {
            weaponSpriteRenderer.sprite = weaponData.weaponDisplay;
        }
        else
        {
            Debug.LogWarning("WeaponSpriteRenderer hoặc weaponDisplay chưa được gán!");
        }
    }
}
