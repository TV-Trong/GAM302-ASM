using UnityEngine;
using System.Collections; 

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform firePos;
    public AudioSource audioSource;
    public Transform player; // Tham chiếu đến Player để xoay luôn

    private float timeBtwFire;

    void Update()
    {
        RotateTowardsMouse();
        timeBtwFire -= Time.deltaTime;

        if (Input.GetMouseButton(0) && timeBtwFire <= 0)
        {
            FireBullet();
        }
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        // Xoay vũ khí theo chuột
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Xoay Player theo chuột
        if (player != null)
        {
            player.rotation = Quaternion.Euler(0, 0, angle);

            // Đảo hướng nếu nhân vật quay ngược
            if (angle > 90 || angle < -90)
                player.localScale = new Vector3(1, -1, 1);
            else
                player.localScale = new Vector3(1, 1, 1);
        }
    }

    void FireBullet()
    {
        if (weaponData == null || weaponData.bulletPrefab == null) return;

        timeBtwFire = weaponData.fireRate;
        GameObject bulletTmp = Instantiate(weaponData.bulletPrefab, firePos.position, transform.rotation);

        Rigidbody2D rb = bulletTmp.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * weaponData.bulletForce, ForceMode2D.Impulse);

        // Phát âm thanh bắn súng
        if (audioSource && weaponData.fireSound)
            audioSource.PlayOneShot(weaponData.fireSound);

        // Nếu là vũ khí nổ thì kích hoạt hẹn giờ phát nổ
        if (weaponData.isExplosive)
            StartCoroutine(ExplodeAfterDelay(bulletTmp, weaponData.explosionData.delay));
    }

    IEnumerator ExplodeAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
