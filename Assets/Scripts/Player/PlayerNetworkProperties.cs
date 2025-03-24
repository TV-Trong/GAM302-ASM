using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNetworkProperties : NetworkBehaviour
{
    TextMeshProUGUI playerName;
    Slider HPSlider;

    [Networked]
    public float BaseHP { get; set; }

    [Networked]
    [HideInInspector]
    public float CurrentHP { get; set; }

    [Networked]
    [HideInInspector]
    public string PlayerName { get; set; }

    ChangeDetector changeDetector;


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

        PlayerName = PlayerPrefs.GetString("LocalName");
        BaseHP = CurrentHP = 100f;
        
        changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }

    public override void Render()
    {
        HPSlider.value = CurrentHP / BaseHP;
        playerName.text = PlayerName;

        foreach (var change in changeDetector.DetectChanges(this, out var previousBuffer, out var currentBuffer))
        {
            Debug.Log("New player");
            break;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void TakeDamageRpc(float damage)
    {
        CurrentHP -= damage;
    }
}