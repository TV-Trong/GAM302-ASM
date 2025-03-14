using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject bulletTrailPrefab; // Prefab vệt đạn
    public float bulletSpeed = 20f;
    public float bulletRange = 10f;
    public int bulletDamage = 20;
    public LayerMask hitLayers;

    private bool isShooting = false; // Trạng thái bắn

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isShooting = true;
            Shoot();
        }
        if (Input.GetMouseButtonUp(0))
        {
            isShooting = false;
        }
    }

    void Shoot()
    {
        Vector2 firePoint = transform.position;
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(firePoint, direction, bulletRange, hitLayers);
        Vector2 targetPoint = hit.collider != null ? hit.point : (firePoint + direction * bulletRange);

        // 🌟 VẼ TIA RAYCAST MÀU VÀNG KHI BẮN
        Debug.DrawRay(firePoint, direction * bulletRange, Color.yellow, 0.1f);

        // 🏹 Tạo vệt đạn
        GameObject bulletTrail = Instantiate(bulletTrailPrefab, firePoint, Quaternion.identity);
        StartCoroutine(MoveTrail(bulletTrail, targetPoint));

        if (hit.collider != null)
        {
            Debug.Log("Bắn trúng: " + hit.collider.name);

            
        }
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Vector2 firePoint = transform.position;
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

        // 🌟 Hiển thị tia Raycast luôn trong Scene View
        Gizmos.color = isShooting ? Color.yellow : Color.blue;
        Gizmos.DrawLine(firePoint, firePoint + direction * bulletRange);
    }

    private IEnumerator MoveTrail(GameObject trail, Vector2 endPoint)
    {
        float time = 0f;
        float duration = Vector2.Distance(trail.transform.position, endPoint) / bulletSpeed;

        while (time < duration)
        {
            trail.transform.position = Vector2.Lerp(trail.transform.position, endPoint, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        trail.transform.position = endPoint;
        Destroy(trail, 0.2f);
    }
}
