using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MAIN_MENU,
        PAUSE_MENU,
        IN_PLAY,
    }

    public static GameObject Instance { get; private set; }

    public GameState currentGameState = GameState.MAIN_MENU;

    [Header("==Loading Screen==")]
    [SerializeField] private GameObject loadingScreenCanvas;
    [SerializeField] private Slider progressBar;
    

    [NonSerialized] public Player player;

    void Awake()
    {
        if (!Instance)
        {
            Instance = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        //InitializeGameStates();

        // TODO: change to be set in inspector
        LoadGameState(GameState.MAIN_MENU);
    }

    public static GameManager GetManager()
    {
        return Instance.GetComponent<GameManager>();
    }

    public void LoadGameState(GameState state)
    {
        AsyncOperation operation;

        switch (state)
        {
            case GameState.MAIN_MENU:
                operation = SceneManager.LoadSceneAsync("MainMenu");
                currentGameState = GameState.MAIN_MENU;

                break;

            case GameState.IN_PLAY:
                LevelManager levelManager = LevelManager.GetManager();
                operation = levelManager.CurrentLevelIndex == 0 ? 
                            levelManager.LoadFirstLevel() : levelManager.LoadNextLevel();
                currentGameState = GameState.IN_PLAY;

                break;
            
            default:
                return;
        }

        loadingScreenCanvas.SetActive(true);
        StartCoroutine(LoadGameStateAsync(operation));
    }

    private IEnumerator LoadGameStateAsync(AsyncOperation operation)
    {
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.95f);
            if (progressBar != null)
            {
                progressBar.value = progress;
            }

            if (operation.progress >= 0.9f)
                loadingScreenCanvas.SetActive(false);

            yield return null; // Wait until the next frame
        }
    }

    // Button Functions
    public void StartGameButton()
    {
        LoadGameState(GameState.IN_PLAY);
    }
}
