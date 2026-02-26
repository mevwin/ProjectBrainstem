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
        LOADING,
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

        LoadGameState(GameState.MAIN_MENU);
    }

    void Update()
    {
        
    }

    public void LoadGameState(GameState state)
    {
        AsyncOperation operation = null;

        switch (state)
        {
            case GameState.MAIN_MENU:
                operation = SceneManager.LoadSceneAsync("MainMenu");

                break;
            
            case GameState.PAUSE_MENU:
                operation = SceneManager.LoadSceneAsync("PauseMenu");

                break;

            case GameState.IN_PLAY:
                // TODO

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


    public static GameManager GetGameManager()
    {
        return Instance.GetComponent<GameManager>();
    }
}
