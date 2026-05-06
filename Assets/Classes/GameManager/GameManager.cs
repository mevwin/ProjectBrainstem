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
        ENTER_DUNGEON,
    }

    public static GameObject Instance { get; private set; }

    public GameState currentGameState = GameState.MAIN_MENU;
    [SerializeField] private GameObject eventSystem;

    [Header("==Loading Screen==")]
    [SerializeField] public LoadingScreen loadingScreen;
    
    [Header("==Pause Screen==")]
    [SerializeField] private GameObject pauseMenuCanvas;
    private InputAction pauseAction;

    [Header("==Debug==")]
    [SerializeField] private bool debugMode;

    public GameObject player;
    public Vector3 playerMainMenuPosition;
    public Quaternion playerManuMenuRotation;
    public Vector3 playerModelForward;
    [SerializeField] private GameObject BlockParent;


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

        if (debugMode)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Destroy(gameObject);
        }
    }

    void Start()
    {
        pauseAction = InputSystem.actions.FindAction("UI/Pause");

        if (player)
        {
            TogglePlayerControls(false);
            playerMainMenuPosition = player.transform.position;
            playerManuMenuRotation = player.transform.rotation;
            playerModelForward = player.transform.forward;
        }

        // TODO: change to be set in inspector
        LoadGameState(GameState.MAIN_MENU);
    }

    void Update()
    {
        if ((currentGameState == GameState.IN_PUZZLE || currentGameState == GameState.IN_HUB)
            && pauseAction.WasPressedThisFrame())
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
        // Destroy all non-player objects attached to the GameManager
        for (int i = BlockParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(BlockParent.transform.GetChild(i).gameObject);
        }

        AsyncOperation operation;

        switch (state)
        {
            case GameState.MAIN_MENU:
                currentGameState = GameState.MAIN_MENU;
                Cursor.lockState = CursorLockMode.Confined;

                operation = SceneManager.LoadSceneAsync("MainMenu");

                TogglePlayerControls(false);
                player.transform.position = playerMainMenuPosition;
                player.transform.rotation = playerManuMenuRotation;
                player.GetComponent<Player>().model.transform.forward = playerModelForward;
                player.SetActive(true);
                
                break;

            case GameState.IN_PUZZLE:
                currentGameState = GameState.IN_PUZZLE;

                LevelManager levelManager = LevelManager.GetManager();
                if (!levelManager.IsShrineComplete())
                    operation = levelManager.LoadNextLevel();
                else 
                {
                    levelManager.currentLevelIndex = 0;
                    operation = SceneManager.LoadSceneAsync("HubWorld");
                }

                break;

            case GameState.IN_HUB:
                currentGameState = GameState.IN_HUB;
                operation = SceneManager.LoadSceneAsync("HubWorld");

                break;
            
            default:
                return;
        }

        if (operation != null)
        {
            loadingScreen.gameObject.SetActive(true);
            StartCoroutine(LoadSceneAsync(operation));
        }
    }

    private IEnumerator LoadSceneAsync(AsyncOperation operation)
    {
        loadingScreen.SetFadeAlpha(1f);
        loadingScreen.progressBar.gameObject.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.95f);
            if (loadingScreen.progressBar != null)
            {
                loadingScreen.progressBar.value = progress;
            }

            if (operation.progress >= 0.9f)
                loadingScreen.progressBar.gameObject.SetActive(false);
        
            yield return null;
        }

        yield return StartCoroutine(loadingScreen.Fade(0));
        loadingScreen.gameObject.SetActive(false);
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
        player.GetComponent<Player>().inMenu = false;
        Time.timeScale = 1f;
        LoadGameState(GameState.MAIN_MENU);
    }

    public void ReturnToHubWorld()
    {
        pauseMenuCanvas.SetActive(false);
        player.SetActive(false);
        Time.timeScale = 1f;
        LevelManager.GetManager().currentLevelIndex = 0;
        LoadGameState(GameState.IN_HUB);
    }

    public void TogglePlayerControls(bool toggle)
    {
        player.GetComponent<Player>().inMenu = !toggle;
        player.GetComponent<Rigidbody>().isKinematic = !toggle;
        player.GetComponent<CameraControl>().enabled = toggle;
    }
}
