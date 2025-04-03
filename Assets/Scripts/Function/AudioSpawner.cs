using Fusion;
using UnityEngine;

public class AudioSpawner : NetworkBehaviour
{
    [SerializeField] GameObject audioManagerPrefab;

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
