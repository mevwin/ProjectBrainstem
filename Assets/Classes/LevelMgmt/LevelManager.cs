using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Public Attributes
    public static GameObject Instance { get; private set; }
    [NonSerialized] public int currentLevelIndex = 0;

    //[SerializeField]
    private DungeonLevelList dungeonList;

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

    public void SetDungeonList(DungeonLevelList list)
    {
        dungeonList = list;
    }

    public AsyncOperation LoadNextLevel()
    {
        return SceneManager.LoadSceneAsync(dungeonList.GetLevelName(currentLevelIndex++));
    }

    public bool IsDungeonComplete()
    {
        return currentLevelIndex == dungeonList.GetSize();
    }

    public static LevelManager GetManager()
    {
        return Instance.GetComponent<LevelManager>();
    }
}
