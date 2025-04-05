using UnityEngine;

public class RandomWeapon : MonoBehaviour
{
    [Header("Prefab vũ khí có thể spawn")]
    public GameObject[] weaponPrefabs;

    [Header("Spawn chỉ một lần?")]
    public bool destroyAfterSpawn = true;

    private bool hasSpawned = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasSpawned) return;

        if (other.CompareTag("Player"))
        {
            if (weaponPrefabs.Length == 0)
            {
                Debug.LogWarning("Chưa gán prefab nào!");
                return;
            }

            int index = Random.Range(0, weaponPrefabs.Length);
            GameObject selected = weaponPrefabs[index];

            Instantiate(selected, transform.position, Quaternion.identity);
            Debug.Log($"Đã spawn vũ khí: {selected.name}");

            hasSpawned = true;

            if (destroyAfterSpawn)
            {
                Destroy(gameObject); // huỷ vùng spawn sau khi đã random xong
            }
        }
    }
}
