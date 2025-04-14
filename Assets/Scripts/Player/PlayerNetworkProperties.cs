using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Fusion.NetworkBehaviour;

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

    [Networked]
    [HideInInspector]
    public int KillCount { get; set; }

    [Networked]
    [HideInInspector]
    public int DeathCount { get; set; }

    [Networked]
    [HideInInspector]
    public int NetworkID { get; set; }

    private Scoreboard scoreboard;
    private ChangeDetector changeDetector;


    private void Update()
    {
        if (HasStateAuthority && Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamageRpc(10);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.transform.localScale = Vector3.one;
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.transform.localScale = Vector3.zero;
        }

        Debug.Log(NetworkID);
    }

    public override void Spawned()
    {
        playerName = GetComponentInChildren<TextMeshProUGUI>();
        HPSlider = GetComponentInChildren<Slider>();

        PlayerName = PlayerPrefs.GetString("LocalName");
        BaseHP = CurrentHP = 100f;

        KillCount = Random.Range(0, 10);
        DeathCount = Random.Range(0, 10);

        scoreboard = FindAnyObjectByType<Scoreboard>();
        scoreboard.AddPlayerScore(this);
        scoreboard.UpdatePlayerScore(this);
        
        changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }

    public override void Render()
    {
        HPSlider.value = CurrentHP / BaseHP;
        playerName.text = PlayerName;

        foreach (var change in changeDetector.DetectChanges(this, out var previousBuffer, out var currentBuffer))
        {
            scoreboard.UpdatePlayerScore(this);
            break;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void TakeDamageRpc(float damage)
    {
        CurrentHP -= damage;

        if (CurrentHP <= 0)
        {
            FindAnyObjectByType<PlayerSpawner>().PlayerRespawn(Object);
            PlayerDieRpc();
        }
    }

    // Thang
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void HealRpc(float healAmount)
    {
        CurrentHP = Mathf.Min(BaseHP, CurrentHP + healAmount); // Hồi máu nhưng không vượt quá giới hạn máu tối đa
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void PlayerDieRpc()
    {
        if (HasInputAuthority)
            gameObject.SetActive(false);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void PlayerRespawnRpc()
    {
        if (HasInputAuthority)
            gameObject.SetActive(true);
    }

}