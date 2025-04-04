using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class AudioPoolObject : NetworkBehaviour
{
    [SerializeField] int poolAmount;
    [SerializeField] NetworkObject audioObjectPrefab;
    List<NetworkObject> poolObjects;

    public override void Spawned()
    {
        poolObjects = new List<NetworkObject>();

        for (int i = 0; i < poolAmount; i++)
        {
            var audioObject = Runner.Spawn(audioObjectPrefab, transform.position, Quaternion.identity);

            audioObject.transform.SetParent(transform);

            var audio = audioObject.AddComponent<AudioSource>();
            audio.playOnAwake = false;

            poolObjects.Add(audioObject);
        }
    }

    public AudioSource GetAudioSource()
    {
        for (int i = 0; i < poolAmount; i++)
        {
            var audio = poolObjects[i].GetComponent<AudioSource>();
            if (audio.isPlaying == false)
                return audio ;
        }

        Debug.LogWarning("Pool not large enough");
        return null;
    }
}

[Serializable]
public class AudioGroup
{
    public string groupName;
    public List<AudioClip> clips;

    public int GetRandomClip()
    {
        return UnityEngine.Random.Range(0, clips.Count);
    }
}
