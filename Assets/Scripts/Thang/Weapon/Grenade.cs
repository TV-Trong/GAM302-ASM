using UnityEngine;
using System.Collections;

public class Grenade : MonoBehaviour
{
    private ExplosionData explosionData;

    public void Initialize(ExplosionData data)
    {
        explosionData = data;
        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionData.delay);

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionData.radius);
        foreach (var obj in hitObjects)
        {
            Debug.Log(obj.name + " bị ảnh hưởng bởi vụ nổ!");
            // Thêm logic sát thương ở đây
        }

        Destroy(gameObject);
    }
}
