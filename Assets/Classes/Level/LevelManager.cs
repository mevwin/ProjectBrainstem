using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<string> levelList = new();

    // Private Vars
    private int currentLevelIndex = 0;

    void Start()
    {
        
    }

    public void LoadLevel(string levelName)
    {
        if (levelList.Contains(levelName)) // Only load scenes that part of the levelList
            SceneManager.LoadSceneAsync(levelName);
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        LoadLevel(levelList[currentLevelIndex]);
    }
}
