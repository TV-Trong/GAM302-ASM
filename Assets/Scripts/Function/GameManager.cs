using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    GameState currentState;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        currentState = GameState.MainMenu;
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (currentState == GameState.Playing)
        {
            // Gọi hàm gì đó khi vào scene chơi game
        }
        else
        {
            // Khi thoát màn chơi thì gọi hàm này
        }
    }
}

public enum GameState
{
    MainMenu,
    Playing
}
