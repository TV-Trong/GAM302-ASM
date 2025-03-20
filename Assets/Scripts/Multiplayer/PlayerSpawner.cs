using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] GameObject playerPrefab;
    LocalPlayer newPlayer;

    private void Start()
    {
        newPlayer = new LocalPlayer
        {
            localName = PlayerPrefs.GetString("LocalName"),
            baseHP = 100f
        };
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            NetworkObject spawnedPlayer = Runner.Spawn(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
            spawnedPlayer.GetComponent<PlayerNetworkProperties>().SetupProperties(newPlayer.localName, newPlayer.baseHP);
        }
    }
}