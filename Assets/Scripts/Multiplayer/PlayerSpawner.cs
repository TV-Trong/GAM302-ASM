using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] float respawnTimer = 3f;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            NetworkObject spawnedPlayer = Runner.Spawn(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        }
    }

    public void PlayerRespawn(PlayerRef localPlayer)
    {
        StartCoroutine(StartSpawningTimer(localPlayer));
    }

    System.Collections.IEnumerator StartSpawningTimer(PlayerRef localPlayer)
    {
        yield return new WaitForSeconds(respawnTimer);
        var playerObject = Runner.Spawn(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity, localPlayer);
    }
}