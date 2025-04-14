using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public List<Transform> spawnPosition = new List<Transform>();
    [SerializeField] private List<GameObject> playersPrefab = new List<GameObject>();
    [SerializeField] float respawnTimer = 3f;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            NetworkObject spawnedPlayer = Runner.Spawn(playersPrefab[PlayerPrefs.GetInt("SkinIndex")], GetSpawnPosition(), Quaternion.identity, inputAuthority: player);
            Runner.SetPlayerObject(player, spawnedPlayer);
            spawnedPlayer.GetComponent<PlayerNetworkProperties>().NetworkID = player.PlayerId;
        }
    }

    public void PlayerRespawn(PlayerRef localPlayer)
    {
        //var player = playerObject.GetComponent<PlayerNetworkProperties>();
        StartCoroutine(StartSpawningTimer(localPlayer));
    }

    System.Collections.IEnumerator StartSpawningTimer(PlayerRef localPlayer)
    {
        yield return new WaitForSeconds(respawnTimer);
        var playerObject = Runner.Spawn(playersPrefab[PlayerPrefs.GetInt("SkinIndex")], GetSpawnPosition(), Quaternion.identity, localPlayer);
        //playerNetworkProperties.PlayerRespawnRpc();
    }

    Vector2 GetSpawnPosition()
    {
        return spawnPosition[Random.Range(0, spawnPosition.Count - 1)].position;
    }
}