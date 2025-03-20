using System;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNetworkProperties : NetworkBehaviour
{
    TextMeshProUGUI playerName;
    Slider HPSlider;

    public float BaseHP { get; set; }

    [Networked, OnChangedRender(nameof(OnTakingDamage))]
    [HideInInspector]
    public float CurrentHP { get; set; }

    [Networked]
    [HideInInspector]
    public string PlayerName { get; set; }

    private void Update()
    {
        if (HasStateAuthority && Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamageRpc(10);
        }
    }

    public override void Spawned()
    {
        playerName = GetComponentInChildren<TextMeshProUGUI>();
        HPSlider = GetComponentInChildren<Slider>();

        if (HasStateAuthority) 
            SetupPropertiesRpc(PlayerPrefs.GetString("LocalName"), 100f);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void SetupPropertiesRpc(string _name, float _baseHP)
    {
        PlayerName = _name;
        BaseHP = CurrentHP = _baseHP;
        playerName.text = PlayerName;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void TakeDamageRpc(float damage)
    {
        CurrentHP -= damage;
    }

    void OnTakingDamage()
    { 
        HPSlider.value = CurrentHP / BaseHP;
    }
}