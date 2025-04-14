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
        if (!Object || hasSpawned) return;

        if (other.CompareTag("Player"))
        {
            // ✅ Chỉ cho phép người có quyền thực hiện
            if (!HasStateAuthority) return;

            if (weaponPrefabs.Length == 0)
            {
                Debug.LogWarning("Chưa gán prefab nào!");
                return;
            }

            int index = Random.Range(0, weaponPrefabs.Length);
            NetworkPrefabRef selected = weaponPrefabs[index];

            Runner.Spawn(selected, transform.position, Quaternion.identity);
            Debug.Log("[Fusion] Đã spawn một vũ khí ngẫu nhiên.");

            hasSpawned = true;

            if (destroyAfterSpawn)
            {
                Runner.Despawn(Object);
            }
        }
    }
}
