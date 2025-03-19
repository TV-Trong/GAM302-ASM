using System.Collections;
using Fusion;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [SerializeField] private WeaponBase weaponData; // Tham chiếu đến ScriptableObject
    [SerializeField] private GameObject bulletTrailPrefab;
    [SerializeField] private SpriteRenderer weaponSpriteRenderer; // SpriteRenderer để hiển thị súng

    //public LayerMask hitLayers;
    private bool isShooting = false;

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

    //public override void FixedUpdateNetwork()
    void Update()
    {
        if (!HasStateAuthority || weaponData == null) return; // Không cho bắn nếu chưa có súng

        if (Input.GetMouseButtonDown(0)) 
        {
            isShooting = true;

            if (weaponData.isShotgun)
            {
                ShootShotgun();
            }
            else
            {
                InvokeRepeating("Shoot", 0, weaponData.fireRate); // Nếu không, bắn như súng thường
            }
        }

        if (Input.GetMouseButtonUp(0)) 
        {
            isShooting = false;
            CancelInvoke("Shoot");
        }
    }

    void Shoot()
    {
        Vector2 firePoint = transform.position;
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPosition - firePoint).normalized;

        RaycastHit2D hit = Physics2D.Raycast(firePoint, direction, weaponData.bulletForce/*, hitLayers*/);
        Vector2 targetPoint = hit.collider != null ? hit.point : (firePoint + direction * weaponData.bulletForce);

        //GameObject bulletTrail = Instantiate(bulletTrailPrefab, firePoint, Quaternion.identity);

        NetworkObject bulletTrail = Runner.Spawn(bulletTrailPrefab, firePoint, Quaternion.identity);

        StartCoroutine(MoveTrail(bulletTrail, targetPoint));

        if (hit.collider != null)
        {
            Debug.Log("Bắn trúng: " + hit.collider.name);
        }
    }

    void ShootShotgun()
    {
        Vector2 firePoint = transform.position;
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPosition - firePoint).normalized;

        int pelletCount = weaponData.GetPelletCount();
        float spreadAngle = weaponData.GetSpreadAngle();
        float halfSpread = (pelletCount - 1) / 2f;

        for (int i = 0; i < pelletCount; i++)
        {
            float angleOffset = (i - halfSpread) * spreadAngle;
            Vector2 spreadDirection = Quaternion.Euler(0, 0, angleOffset) * direction;

            RaycastHit2D hit = Physics2D.Raycast(firePoint, spreadDirection, weaponData.bulletForce);
            Vector2 targetPoint = hit.collider != null ? hit.point : (firePoint + spreadDirection * weaponData.bulletForce);

            NetworkObject bulletTrail = Runner.Spawn(bulletTrailPrefab, firePoint, Quaternion.identity);

            StartCoroutine(MoveTrail(bulletTrail, targetPoint));

            if (hit.collider != null)
            {
                Debug.Log("Shotgun trúng: " + hit.collider.name);
            }
        }
    }

    void OnDrawGizmos()
    {
        //if (!Application.isPlaying) return;

        //Vector2 firePoint = transform.position;
        //Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 direction = (mouseWorldPosition - firePoint).normalized;

        //Gizmos.color = isShooting ? Color.yellow : Color.blue;
        //Gizmos.DrawLine(firePoint, firePoint + direction * weaponData.bulletForce);
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
        Destroy(trail, 0.2f);
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
