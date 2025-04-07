using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemPrefabs;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private int maxItems = 10;

    private Dictionary<Transform, GameObject> activeItems = new Dictionary<Transform, GameObject>();

    void Start()
    {
        SpawnInitialItems();
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
        GameObject prefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];

        GameObject itemInstance = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        activeItems.Add(spawnPoint, itemInstance);

        Debug.Log($"Spawn item mới: {itemInstance.name} tại {spawnPoint.name}");

        // Gắn callback cho cả ItemPickup và WeaponPickup
        ItemPickup itemPickup = itemInstance.GetComponent<ItemPickup>();
        if (itemPickup != null)
        {
            itemPickup.OnItemPicked += () => OnItemPicked(spawnPoint);
            return;
        }

        WeaponPickup weaponPickup = itemInstance.GetComponent<WeaponPickup>();
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
            Debug.Log($"Item bị nhặt tại {spawnPoint.name}, spawn item mới");
            activeItems.Remove(spawnPoint);
            SpawnNewItem();
        }
    }
}
