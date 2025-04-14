using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private GameObject playerScorePrefab;
    [SerializeField] private Transform parent;

    private Dictionary<int, GameObject> scoreToObjectDict = new Dictionary<int, GameObject>();

    public void Clear()
    {
        foreach (var scoreObject in scoreToObjectDict.Values)
        {
            Destroy(scoreObject);
        }
        scoreToObjectDict.Clear();
    }

    public void AddPlayerScore(PlayerNetworkProperties playerNetworkProperties)
    {
        if (scoreToObjectDict.ContainsKey(playerNetworkProperties.NetworkID) || playerNetworkProperties == null)
        {
            Debug.LogWarning("Player already exists in scoreboard or Is null");
            return;
        }

        int id = playerNetworkProperties.NetworkID;

        GameObject scoreObject = Instantiate(playerScorePrefab, parent);
        scoreToObjectDict.Add(id, scoreObject);

        UpdatePlayerScore(playerNetworkProperties);
    }

    public void UpdatePlayerScore(PlayerNetworkProperties playerNetworkProperties)
    {
        Debug.Log("Scoreboard: " + playerNetworkProperties.NetworkID);

        if (scoreToObjectDict.TryGetValue(playerNetworkProperties.NetworkID, out GameObject scoreObject))
        {
            scoreObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerNetworkProperties.PlayerName;
            scoreObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = playerNetworkProperties.KillCount.ToString();
            scoreObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = playerNetworkProperties.DeathCount.ToString();
        }
        else
        {
            Debug.LogWarning("Player not found in scoreboard");
        }
    }

    public void RemovePlayerScore(PlayerNetworkProperties playerNetworkProperties)
    {
        if (scoreToObjectDict.TryGetValue(playerNetworkProperties.NetworkID, out GameObject scoreObject))
        {
            if (scoreObject == null)
            {
                Debug.LogWarning("Score object is null");
                return;
            }

            Destroy(scoreObject);
            scoreToObjectDict.Remove(playerNetworkProperties.NetworkID);
        }
        else
        {
            Debug.LogWarning("Player not found in scoreboard");
        }
    }
}
