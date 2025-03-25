using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public List<Transform> spawnPosition = new List<Transform>();
    [SerializeField] GameObject playerPrefab;
    [SerializeField] float respawnTimer = 3f;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            NetworkObject spawnedPlayer = Runner.Spawn(playerPrefab, GetSpawnPosition(), Quaternion.identity);
        }
    }

    public void PlayerRespawn(PlayerRef localPlayer)
    {
        StartCoroutine(StartSpawningTimer(localPlayer));
    }

    System.Collections.IEnumerator StartSpawningTimer(PlayerRef localPlayer)
    {
        yield return new WaitForSeconds(respawnTimer);
        var playerObject = Runner.Spawn(playerPrefab, GetSpawnPosition(), Quaternion.identity, localPlayer);
    }

    Vector2 GetSpawnPosition()
    {
        return spawnPosition[Random.Range(0, spawnPosition.Count - 1)].position;
    }
}