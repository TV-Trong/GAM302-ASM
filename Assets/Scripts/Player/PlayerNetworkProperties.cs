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
    }

    public override void Render()
    {
        HPSlider.value = CurrentHP / BaseHP;
        playerName.text = PlayerName;
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

    // thang
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void HealRpc(float healAmount)
    {
        CurrentHP = Mathf.Min(BaseHP, CurrentHP + healAmount); // Hồi máu nhưng không vượt quá giới hạn máu tối đa
    }
}