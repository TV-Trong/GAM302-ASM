using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private List<RankPart> ranks = new List<RankPart>();

    [SerializeField] private Image rankPart1;
    [SerializeField] private Image rankPart2;
    [SerializeField] private Image rankPart3;
    [SerializeField] private TextMeshProUGUI rankName;

    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private TextMeshProUGUI playerNamePlaceholder;

    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private string savePath = "Assets/SaveFolder/Save.json";

    Dictionary<int, string> indexToRankDict = new Dictionary<int, string>();

    private int rankIndex;

    private void Awake()
    {
        rankIndex = 0;

        indexToRankDict.Add(0, "Bronze");
        indexToRankDict.Add(1, "Silver");
        indexToRankDict.Add(2, "Emerald");
        indexToRankDict.Add(3, "Diamond");

        rankPart1.sprite = ranks[rankIndex].part1;
        rankPart2.sprite = ranks[rankIndex].part2;
        rankPart3.sprite = ranks[rankIndex].part3;
        rankName.text = $"Your Rank:\n{indexToRankDict[rankIndex]}";
    }

    public void NextRank()
    {
        rankIndex++;
        if (rankIndex == ranks.Count)
            rankIndex = 0;

        rankPart1.sprite = ranks[rankIndex].part1;
        rankPart2.sprite = ranks[rankIndex].part2;
        rankPart3.sprite = ranks[rankIndex].part3;
        rankName.text = $"Your Rank:\n{indexToRankDict[rankIndex]}";
    }

    public void PrevRank()
    {
        rankIndex--;
        if (rankIndex < 0)
            rankIndex = ranks.Count - 1;

        rankPart1.sprite = ranks[rankIndex].part1;
        rankPart2.sprite = ranks[rankIndex].part2;
        rankPart3.sprite = ranks[rankIndex].part3;
        rankName.text = $"Your Rank:\n{indexToRankDict[rankIndex]}";
    }

    public void SaveData()
    {
        try
        {
            if (!Directory.Exists("Assets/SaveFolder"))
            {
                Directory.CreateDirectory("Assets/SaveFolder");
            }

            if (!File.Exists(savePath))
            {
                File.WriteAllText(savePath, "");
            }

            Data data = new Data();
            data.PlayerName = (String.IsNullOrWhiteSpace(playerNameInputField.text)) ? playerNamePlaceholder.text : playerNameInputField.text;
            data.Rank = rankIndex.ToString();
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(savePath, json);

            Debug.Log("Save Success");
        }
        catch
        {
            Debug.Log("Save Failed");
        }
    }

    public void LoadData()
    {
        try
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                Data data = JsonUtility.FromJson<Data>(json);
                playerNameInputField.text = data.PlayerName;
                rankIndex = int.Parse(data.Rank);
                rankPart1.sprite = ranks[rankIndex].part1;
                rankPart2.sprite = ranks[rankIndex].part2;
                rankPart3.sprite = ranks[rankIndex].part3;
                rankName.text = $"Your Rank:\n{indexToRankDict[rankIndex]}";
                Debug.Log(rankIndex);
            }
        }
        catch
        {
            Debug.Log("Load Failed");   
        }
    }
}

[Serializable]
public class Data
{
    public string PlayerName;
    public string Rank;
}

[Serializable]
public class RankPart
{
    public Sprite part1;
    public Sprite part2;
    public Sprite part3;
}
