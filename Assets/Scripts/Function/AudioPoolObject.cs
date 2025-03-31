using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioPoolObject : MonoBehaviour
{
    [SerializeField] int poolAmount;
    List<AudioSource> poolObjects;

    private void Start()
    {
        poolObjects = new List<AudioSource>();

        for (int i = 0; i < poolAmount; i++)
        {
            var audio = transform.AddComponent<AudioSource>();
            audio.playOnAwake = false;
            poolObjects.Add(audio);
        }
    }

    public AudioSource GetAudioSource()
    {
        for (int i = 0; i < poolAmount; i++)
        {
            if (poolObjects[i].isPlaying == false)
                return poolObjects[i];
        }

        Debug.LogWarning("Pool not large enough");
        return null;
    }
}
