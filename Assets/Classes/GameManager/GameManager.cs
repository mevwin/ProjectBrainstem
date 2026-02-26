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
        IN_PLAY,
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

        if (eventSystem)
            DontDestroyOnLoad(eventSystem);
    }

    void Start()
    {
        pauseAction = InputSystem.actions.FindAction("UI/Pause");

        // TODO: change to be set in inspector
        LoadGameState(GameState.MAIN_MENU);
    }

    void Update()
    {
        if (currentGameState == GameState.IN_PLAY && pauseAction.WasPressedThisFrame())
        {
            TogglePauseMenu();
        }
    }

    public static GameManager GetManager()
    {
        return Instance.GetComponent<GameManager>();
    }

    // Async Loading Functions
    public void LoadGameState(GameState state)
    {
        AsyncOperation operation;

        switch (state)
        {
            case GameState.MAIN_MENU:
                currentGameState = GameState.MAIN_MENU;
                Cursor.lockState = CursorLockMode.None;

                operation = SceneManager.LoadSceneAsync("MainMenu");
                
                break;

            case GameState.IN_PLAY:
                currentGameState = GameState.IN_PLAY;

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
        LoadGameState(GameState.IN_PLAY);
    }

    // Pause Menu
    public void TogglePauseMenu()
    {
        Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
        Cursor.lockState = Time.timeScale == 0f ? CursorLockMode.None : CursorLockMode.Confined;
        pauseMenuCanvas.SetActive(Time.timeScale == 0f);
    }

    public void ReturnToMainMenu()
    {
        pauseMenuCanvas.SetActive(false);
        LoadGameState(GameState.MAIN_MENU);
    }
}
