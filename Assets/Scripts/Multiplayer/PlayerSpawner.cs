using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] GameObject playerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            NetworkObject spawnedPlayer = Runner.Spawn(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
            Runner.SetPlayerObject(player, spawnedPlayer);
        }
    }
}