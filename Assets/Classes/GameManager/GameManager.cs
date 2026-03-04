using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MAIN_MENU,
        IN_HUB,
        IN_PUZZLE,
    }

    public static GameObject Instance { get; private set; }

    public GameState currentGameState = GameState.MAIN_MENU;
    [SerializeField] private GameObject eventSystem;

    [Header("==Loading Screen==")]
    [SerializeField] private GameObject loadingScreenCanvas;
    [SerializeField] private Slider progressBar;
    
    [Header("==Pause Screen==")]
    [SerializeField] private GameObject pauseMenuCanvas;
    private InputAction pauseAction;

    public GameObject player;


    void Awake()
    {
        if (!Instance)
        {
            Instance = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        if (eventSystem)
            DontDestroyOnLoad(eventSystem);
    }

    void Start()
    {
        pauseAction = InputSystem.actions.FindAction("UI/Pause");

        if (player)
            player.SetActive(false);

        // TODO: change to be set in inspector
        LoadGameState(GameState.MAIN_MENU);
    }

    void Update()
    {
        if (currentGameState == GameState.IN_PUZZLE && pauseAction.WasPressedThisFrame())
        {
            TogglePauseMenu();
        }
    }

    public void GameComplete()
    {
        /**
        If game is complete:
            go back to main menu
            deload player
        */


    }

    public static GameManager GetManager()
    {   
        if (Instance)
            return Instance.GetComponent<GameManager>();
        return null;
    }

    // Async Loading Functions
    public void LoadGameState(GameState state)
    {
        AsyncOperation operation;

        switch (state)
        {
            case GameState.MAIN_MENU:
                currentGameState = GameState.MAIN_MENU;
                Cursor.lockState = CursorLockMode.Confined;

                operation = SceneManager.LoadSceneAsync("MainMenu");
                
                break;

            case GameState.IN_HUB:
                currentGameState = GameState.IN_HUB;
                operation = SceneManager.LoadSceneAsync("HubWorld");

                break;

            case GameState.IN_PUZZLE:
                currentGameState = GameState.IN_PUZZLE;

                LevelManager levelManager = LevelManager.GetManager();
                operation = levelManager.LoadFirstLevel(); // TODO: change later

                break;
            
            default:
                return;
        }

        if (operation != null)
        {
            loadingScreenCanvas.SetActive(true);
            StartCoroutine(LoadGameStateAsync(operation));
        }
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

            yield return null;
        }
    }

    /** 
     * MENU BUTTON FUNCTIONS
     */

    // Main Menu
    public void StartGame()
    {
        LoadGameState(GameState.IN_HUB);
    }

    // Pause Menu
    public void TogglePauseMenu()
    {
        Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
        Cursor.lockState = Time.timeScale == 0f ? CursorLockMode.Confined : CursorLockMode.Locked;
        pauseMenuCanvas.SetActive(Time.timeScale == 0f);
    }

    public void ReturnToMainMenu()
    {
        pauseMenuCanvas.SetActive(false);
        player.SetActive(false);
        Time.timeScale = 1f;
        LoadGameState(GameState.MAIN_MENU);
    }
}
