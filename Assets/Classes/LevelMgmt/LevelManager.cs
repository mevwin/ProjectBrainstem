using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Public Attributes
    public static GameObject Instance { get; private set; }
    public int CurrentLevelIndex { get; private set; } = 0;

    [SerializeField] private List<string> levelList = new();


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
        
    }

    public AsyncOperation LoadLevel(string levelName)
    {
        if (levelList.Contains(levelName)) // Only load scenes that part of the levelList
            return SceneManager.LoadSceneAsync(levelName);

        return null;
    }

    public AsyncOperation LoadFirstLevel()
    {
        return LoadLevel(levelList[0]);
    }

    public AsyncOperation LoadNextLevel()
    {
        CurrentLevelIndex++;
        return LoadLevel(levelList[CurrentLevelIndex]);
    }

    public static LevelManager GetManager()
    {
        return Instance.GetComponent<LevelManager>();
    }
}
