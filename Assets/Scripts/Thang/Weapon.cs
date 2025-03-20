using System.Collections;
using Fusion;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [SerializeField] private WeaponBase weaponData; // Tham chiếu đến ScriptableObject
    [SerializeField] private GameObject bulletTrailPrefab;
    [SerializeField] private SpriteRenderer weaponSpriteRenderer; // SpriteRenderer để hiển thị súng
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform firePoint;

    void Start()
    {
        if (weaponData == null)
        {
            Debug.LogWarning("weaponData chưa được gán, cần nhặt vũ khí!");
        }
        else
        {
            UpdateWeaponSprite(); // Hiển thị hình ảnh súng ban đầu nếu có
        }
    }

    void Update()
    {
        if (!HasStateAuthority || weaponData == null) return; // Không cho bắn nếu chưa có súng

        if (Input.GetMouseButtonDown(0)) 
        {
            if (weaponData.isShotgun)
            {
                InvokeRepeating("ShootShotgun", 0, weaponData.fireRate);
            }
            else
            {
                InvokeRepeating("Shoot", 0, weaponData.fireRate); // Nếu không, bắn như súng thường
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
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPosition - (Vector2)playerTransform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast((Vector2)firePoint.position, direction, weaponData.bulletForce);

        Vector2 targetPoint = hit.collider != null ? hit.point : ((Vector2)firePoint.position + direction * weaponData.bulletForce);

        NetworkObject bulletTrail = Runner.Spawn(bulletTrailPrefab, (Vector2)firePoint.position, Quaternion.identity);

        StartCoroutine(MoveTrail(bulletTrail, targetPoint));

        if (hit.collider != null)
        {
            HitPlayer(hit);
        }
    }

    private void HitPlayer(RaycastHit2D hit)
    {
        if (hit.transform.CompareTag("Player") && hit.transform != transform.root)
        {
            hit.transform.GetComponent<PlayerNetworkProperties>().TakeDamageRpc(10);
            Debug.Log("Damage enemy");
        }
    }

    void ShootShotgun()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPosition - (Vector2)playerTransform.position).normalized;

        int pelletCount = weaponData.GetPelletCount();
        float spreadAngle = weaponData.GetSpreadAngle();
        float halfSpread = (pelletCount - 1) / 2f;

        for (int i = 0; i < pelletCount; i++)
        {
            float angleOffset = (i - halfSpread) * spreadAngle;
            Vector2 spreadDirection = Quaternion.Euler(0, 0, angleOffset) * direction;

            RaycastHit2D hit = Physics2D.Raycast((Vector2)firePoint.position, spreadDirection, weaponData.bulletForce);
            Vector2 targetPoint = hit.collider != null ? hit.point : ((Vector2)firePoint.position + spreadDirection * weaponData.bulletForce);

            NetworkObject bulletTrail = Runner.Spawn(bulletTrailPrefab, (Vector2)firePoint.position, Quaternion.identity);

            StartCoroutine(MoveTrail(bulletTrail, targetPoint));

            if (hit.collider != null)
            {
                HitPlayer(hit);
            }
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
        Debug.Log("Trang bị vũ khí: " + weaponData.name);
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
