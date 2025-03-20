using System;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNetworkProperties : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] Slider HPSlider;

    [Networked, OnChangedRender(nameof(OnTakingDamage))]
    [HideInInspector]
    public float BaseHP { get; set; }
    public float CurrentHP { get; set; }
    public string PlayerName { get; set; }

    public override void Spawned()
    {
        playerName.text = PlayerName;
        HPSlider.value = 1;
    }

    public void SetupProperties(string _name, float _baseHP)
    {
        PlayerName = _name;
        BaseHP = CurrentHP = _baseHP;
    }

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
    }

    void OnTakingDamage()
    {
        HPSlider.value = CurrentHP / BaseHP;
    }
}

public struct LocalPlayer
{
    public string localName;
    public float baseHP;
}