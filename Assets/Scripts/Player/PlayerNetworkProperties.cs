using System;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNetworkProperties : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI playerName;
    Slider HPSlider;

    public float BaseHP { get; set; }

    [Networked, OnChangedRender(nameof(OnTakingDamage))]
    [HideInInspector]
    public float CurrentHP { get; set; }

    [Networked, OnChangedRender(nameof(OnNameChange))]
    [HideInInspector]
    public string PlayerName { get; set; }

    private void Update()
    {
        if (HasStateAuthority && Input.GetKeyDown(KeyCode.Space))
        {
            PlayerName = PlayerPrefs.GetString("LocalName");
            TakeDamage(10);
            Debug.Log(CurrentHP);
        }
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
            SetupProperties(PlayerPrefs.GetString("LocalName"), 100f);

        HPSlider = GetComponentInChildren<Slider>();
    }

    public void SetupProperties(string _name, float _baseHP)
    {
        PlayerName = _name;
        BaseHP = CurrentHP = _baseHP;
        playerName.text = PlayerName;
    }

    void OnNameChange()
    {
        playerName.text = PlayerName;
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