using Fusion;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] NetworkRunner runnerPrefab;
    NetworkRunner runnerInstance = null;

    [SerializeField] TMP_InputField playerNameInput = null;
    [SerializeField] TMP_InputField roomNameInput = null;
    [SerializeField] TextMeshProUGUI playerNamePlaceHolder;
    [SerializeField] TextMeshProUGUI roomNamePlaceHolder;
    [SerializeField] string playScene;
    
    GameManager gameManager;

    private void Start()
    {
        int randomPlayer = Random.Range(1, 1001);
        int randomRoom = Random.Range(1, 101);

        playerNamePlaceHolder.text = $"Agent {randomPlayer}";
        roomNamePlaceHolder.text = $"Room {randomRoom}";

        gameManager = GameManager.Instance;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void JoinRoom()
    {
        string roomName = string.IsNullOrWhiteSpace(roomNameInput.text) ? roomNamePlaceHolder.text : roomNameInput.text;
        SetLocalName();

        StartGame(GameMode.Shared, roomName, playScene);
    }

    private void SetLocalName()
    {
        string localName = string.IsNullOrWhiteSpace(playerNameInput.text) ? playerNamePlaceHolder.text : playerNameInput.text;

        PlayerPrefs.SetString("LocalName", localName);
    }

    public async void StartGame(GameMode mode, string roomName, string playScene)
    {
        if (runnerInstance == null)
        {
            runnerInstance = Instantiate(runnerPrefab);
        }

        var newGameArgs = new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            Scene = SceneRef.FromIndex(1),
            ObjectProvider = runnerInstance.GetComponent<NetworkObjectPooler>()
        };

        await runnerInstance.StartGame(newGameArgs);

        gameManager.currentState = GameState.Playing;

        if (runnerInstance.IsServer)
        {
            runnerInstance.LoadScene("Gameplay");
        }
    }
}
