using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioPoolObject poolAudio;
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] List<AudioGroup> audioGroups;
    Dictionary<string, AudioGroup> clipDict = new Dictionary<string, AudioGroup>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var group in audioGroups)
        {
            if (group != null)
                clipDict[group.groupName] = group;
            else
                Debug.LogWarning("Some clips is empty!");
        }
    }

    public void PlayAudio(string groupName, AudioType type)
    {
        Debug.Log("OK");

        if (!clipDict.TryGetValue(groupName, out var group))
        {
            Debug.LogWarning($"Audio clip '{groupName}' not found!");
            return;
        }

        Debug.Log(group == null);
        Debug.Log(group.GetRandomClip());
        var audio = poolAudio.GetAudioSource();

        if (audio != null )
        {
            audio.Stop();
            audio.clip = group.clips[group.GetRandomClip()];
            audio.outputAudioMixerGroup = audioMixer.FindMatchingGroups(type.ToString())[0];
            audio.Play();
        }
    }
}

public enum AudioType
{
    SFX,
    BGM
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
