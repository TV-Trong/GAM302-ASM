using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNetworkProperties : NetworkBehaviour
{
    TextMeshProUGUI playerName;
    Slider HPSlider;
    [SerializeField] GameObject light2D;
    [SerializeField] GameObject playerGUI;

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

        if (HasStateAuthority)
            light2D.SetActive(true);

        PlayerName = PlayerPrefs.GetString("LocalName");
        BaseHP = CurrentHP = 100f;

        TogglePlayerGUI();
        
        changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }

    public override void Render()
    {
        HPSlider.value = CurrentHP / BaseHP;
        playerName.text = PlayerName;

        //foreach (var change in changeDetector.DetectChanges(this, out var previousBuffer, out var currentBuffer))
        //{
        //    break;
        //}
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void TakeDamageRpc(float damage)
    {
        CurrentHP -= damage;

        if (CurrentHP <= 0)
        {
            FindAnyObjectByType<PlayerSpawner>().PlayerRespawn(Runner.LocalPlayer);
            Runner.Despawn(Object);
        }
    }

    public void TogglePlayerGUI(bool isOn = false)
    {
        if (!HasStateAuthority)
        {
            playerGUI.SetActive(isOn);
        }
    }
}