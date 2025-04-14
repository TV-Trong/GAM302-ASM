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
            NetworkObject spawnedPlayer = Runner.Spawn(playerPrefab, GetSpawnPosition(), Quaternion.identity, inputAuthority: player);
            Runner.SetPlayerObject(player, spawnedPlayer);
            spawnedPlayer.GetComponent<PlayerNetworkProperties>().NetworkID = player.PlayerId;
        }
    }

    public void PlayerRespawn(NetworkObject playerObject)
    {
        var player = playerObject.GetComponent<PlayerNetworkProperties>();
        StartCoroutine(StartSpawningTimer(player));
    }

    System.Collections.IEnumerator StartSpawningTimer(PlayerNetworkProperties playerNetworkProperties)
    {
        yield return new WaitForSeconds(respawnTimer);
        playerNetworkProperties.PlayerRespawnRpc();
    }

    Vector2 GetSpawnPosition()
    {
        return spawnPosition[Random.Range(0, spawnPosition.Count - 1)].position;
    }
}