using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponBase weaponData; // Tham chiếu đến ScriptableObject
    [SerializeField] private GameObject bulletTrailPrefab;
    public LayerMask hitLayers;
    private bool isShooting = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isShooting = true;
            InvokeRepeating("Shoot", 0, weaponData.fireRate);
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

        RaycastHit2D hit = Physics2D.Raycast(firePoint, direction, weaponData.bulletForce, hitLayers);
        Vector2 targetPoint = hit.collider != null ? hit.point : (firePoint + direction * weaponData.bulletForce);

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
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPosition - firePoint).normalized;

        Gizmos.color = isShooting ? Color.yellow : Color.blue;
        Gizmos.DrawLine(firePoint, firePoint + direction * weaponData.bulletForce);
    }

    private IEnumerator MoveTrail(GameObject trail, Vector2 endPoint)
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
}
