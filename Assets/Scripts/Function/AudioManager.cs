using System;
using System.Collections.Generic;
using Fusion;
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

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void PlayAudioRpc(string groupName, string audioType)
    {
        if (!clipDict.TryGetValue(groupName, out var group))
        {
            Debug.LogWarning($"Audio clip '{groupName}' not found!");
            return;
        }

        var audio = poolAudio.GetAudioSource();

        if (audio != null )
        {
            audio.Stop();
            audio.clip = group.clips[group.GetRandomClip()];
            audio.outputAudioMixerGroup = audioMixer.FindMatchingGroups(audioType)[0];
            audio.Play();
        }
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
