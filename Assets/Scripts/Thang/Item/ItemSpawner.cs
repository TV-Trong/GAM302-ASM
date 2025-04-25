using Fusion;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemSpawner : NetworkBehaviour
{
    [SerializeField] private List<GameObject> itemPrefabs;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private int maxItems = 10;

    private Dictionary<Transform, NetworkObject> activeItems = new Dictionary<Transform, NetworkObject>();

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            SpawnInitialItems();
        }
    }

    void SpawnInitialItems()
    {
        while (activeItems.Count < maxItems)
        {
            SpawnNewItem();
        }
    }

    void SpawnNewItem()
    {
        var availablePoints = spawnPoints.Where(p => !activeItems.ContainsKey(p)).ToList();
        if (availablePoints.Count == 0) return;

        Transform spawnPoint = availablePoints[Random.Range(0, availablePoints.Count)];

        var randomIndex = Random.Range(0, 100);

        GameObject prefab = new GameObject();

        if (randomIndex < 25)
        {
            prefab = itemPrefabs[0];
        }
        else if (randomIndex < 50)
        {
            prefab = itemPrefabs[1];
        }
        else
        {
            prefab = itemPrefabs[2];
        }

            NetworkObject itemNetworkObj = Runner.Spawn(prefab, spawnPoint.position, Quaternion.identity);
        activeItems.Add(spawnPoint, itemNetworkObj);

        Debug.Log($"Spawn item mới: {itemNetworkObj.name} tại {spawnPoint.name}");

        var itemPickup = itemNetworkObj.GetComponent<ItemPickup>();
        if (itemPickup != null)
        {
            itemPickup.OnItemPicked += () => OnItemPicked(spawnPoint);
            return;
        }

        var weaponPickup = itemNetworkObj.GetComponent<WeaponPickup>();
        if (weaponPickup != null)
        {
            weaponPickup.OnItemPicked += () => OnItemPicked(spawnPoint);
            return;
        }

        Debug.LogWarning("Prefab không có ItemPickup hoặc WeaponPickup");
    }

    void OnItemPicked(Transform spawnPoint)
    {
        if (activeItems.ContainsKey(spawnPoint))
        {
            var item = activeItems[spawnPoint];
            Debug.Log($"Item bị nhặt tại {spawnPoint.name}, despawn và spawn mới");

            Runner.Despawn(item);
            activeItems.Remove(spawnPoint);
            SpawnNewItem();
        }
    }
}
