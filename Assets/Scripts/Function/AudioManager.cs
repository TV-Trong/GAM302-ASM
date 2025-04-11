using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : NetworkBehaviour
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

    public override void Spawned()
    {
        foreach (var group in audioGroups)
        {
            if (group != null)
                clipDict[group.groupName] = group;
            else
                Debug.LogWarning("Some clips is empty!");
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void PlayAudioRpc(string groupName, string audioType, Vector3 soundPosition)
    {
        if (!clipDict.TryGetValue(groupName, out var group))
        {
            Debug.LogWarning($"Audio clip '{groupName}' not found!");
            return;
        }

        var audio = poolAudio.GetAudioSource();
        audio.transform.position = soundPosition;

        if (audio != null )
        {
            audio.Stop();
            audio.clip = group.clips[group.GetRandomClip()];
            audio.outputAudioMixerGroup = audioMixer.FindMatchingGroups(audioType)[0];
            audio.Play();
        }
    }
}
