using UnityEngine;
using Fusion;

public class RandomWeapon : NetworkBehaviour
{
    [Header("Prefab vũ khí có thể spawn (đã đăng ký trong NetworkProjectConfig)")]
    public NetworkPrefabRef[] weaponPrefabs;

    [Header("Spawn chỉ một lần?")]
    public bool destroyAfterSpawn = true;

    private bool hasSpawned = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasSpawned) return;

        if (!Object.HasStateAuthority) return; // ✅ Chỉ host có quyền spawn

        if (other.CompareTag("Player"))
        {
            if (weaponPrefabs.Length == 0)
            {
                Debug.LogWarning("Chưa gán prefab nào!");
                return;
            }

            int index = Random.Range(0, weaponPrefabs.Length);
            NetworkPrefabRef selected = weaponPrefabs[index];

            // ✅ Host spawn vũ khí
            Runner.Spawn(selected, transform.position, Quaternion.identity);
            Debug.Log("[Fusion] Đã spawn một vũ khí ngẫu nhiên tại vị trí đúng.");

            hasSpawned = true;

            if (destroyAfterSpawn)
            {
                Runner.Despawn(Object); // cũng xóa bộ random này luôn
            }
        }
    }

}
