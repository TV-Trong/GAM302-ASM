using Fusion;
using UnityEngine;

public class AudioSpawner : NetworkBehaviour
{
    [SerializeField] GameObject audioManagerPrefab;
    //NetworkRunner networkRunner;

    //private void Start()
    //{
    //    networkRunner = FindFirstObjectByType<NetworkRunner>();

    //    if (networkRunner != null)
    //        SpawnAudioManager();
    //    else
    //        Debug.LogWarning("NetworkRunner not found!");
    //}
    public override void Spawned()
    {
        if (Object.HasStateAuthority)  // Only the host should spawn it
        {
            SpawnAudioManager();
        }
    }

    private void SpawnAudioManager()
    {
        if (Runner == null)
        {
            Debug.LogError("NetworkRunner is null!");
            return;
        }

        Runner.Spawn(audioManagerPrefab, Vector3.zero, Quaternion.identity);
    }
}
