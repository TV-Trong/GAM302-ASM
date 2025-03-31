using System.Collections;
using Fusion;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [SerializeField] public WeaponBase weaponData; // Dữ liệu vũ khí từ ScriptableObject
    [SerializeField] private GameObject bulletTrailPrefab; // Hiệu ứng đường đạn
    [SerializeField] private SpriteRenderer weaponSpriteRenderer; // Hình ảnh súng
    [SerializeField] private Transform playerTransform; // Vị trí người chơi
    [SerializeField] private Transform firePoint; // Điểm bắn đạn

    [SerializeField] LayerMask ignoredLayer;

    private bool canShoot = false;

    void Start()
    {
        if (weaponData == null)
        {
            Debug.LogWarning("Chưa có vũ khí! Cần nhặt vũ khí trước khi bắn.");
            canShoot = false;
        }
        else
        {
            UpdateWeaponSprite();
            canShoot = true;
            weaponData.currentAmmo = weaponData.maxAmmo; // Reset lại số đạn khi khởi tạo
        }
    }

    void Update()
    {
        if (!HasStateAuthority || !canShoot || weaponData == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();

            if (weaponData.isInfiniteAmmo || weaponData.currentAmmo > 0)
            {
                inventory.UseAmmo(weaponData);

                if (weaponData.isShotgun)
                {
                    InvokeRepeating("ShootShotgun", 0, weaponData.fireRate);
                }
                else
                {
                    InvokeRepeating("Shoot", 0, weaponData.fireRate);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke("Shoot");
            CancelInvoke("ShootShotgun");
        }
    }

    void Shoot()
    {
        if (weaponData.currentAmmo <= 0 && !weaponData.isInfiniteAmmo) return;

        AudioManager.Instance.PlayAudio("AK47", AudioType.SFX);

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
        if (weaponData.currentAmmo <= 0 && !weaponData.isInfiniteAmmo) return;

        Vector2 mouseWorldPosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPosition - (Vector2)playerTransform.position).normalized;

        int pelletCount = weaponData.GetPelletCount();
        float spreadAngle = weaponData.GetSpreadAngle();
        float halfSpread = (pelletCount - 1) / 2f;

        for (int i = 0; i < pelletCount; i++)
        {
            float angleOffset = (i - halfSpread) * spreadAngle;
            Vector2 spreadDirection = Quaternion.Euler(0, 0, angleOffset) * direction;

            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, spreadDirection, weaponData.bulletForce, ~ignoredLayer);
            Vector2 targetPoint = hit.collider != null ? hit.point : (Vector2)firePoint.position + spreadDirection * weaponData.bulletForce;

            NetworkObject bulletTrail = Runner.Spawn(bulletTrailPrefab, firePoint.position, Quaternion.identity);
            StartCoroutine(MoveTrail(bulletTrail, targetPoint));

            if (hit.collider != null)
            {
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
