using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector] public GameState currentState;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentState = GameState.MainMenu;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (currentState == GameState.Playing)
        {
            PlayerSpawner playerSpawner = FindAnyObjectByType<PlayerSpawner>();
            GameObject spawnPoints = GameObject.FindWithTag("SpawnPoint");

            foreach (Transform spawnPoint in spawnPoints.transform)     
            {
                playerSpawner.spawnPosition.Add(spawnPoint);
            }
        }
        else
        {
            // Khi thoát màn chơi thì gọi hàm này
        }
    }

    private void OnApplicationQuit()
    {
        currentState = GameState.MainMenu;
    }
}

public enum GameState
{
    MainMenu,
    Playing
}
