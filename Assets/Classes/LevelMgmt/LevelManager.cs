using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Public Attributes
    public static GameObject Instance { get; private set; }
    [NonSerialized] public int currentLevelIndex = 0;

    [SerializeField]
    private ShrineLevelList shrineList;

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

    public void SetShrineList(ShrineLevelList list)
    {
        shrineList = list;
    }

    public AsyncOperation LoadNextLevel()
    {
        return SceneManager.LoadSceneAsync(shrineList.GetLevelName(currentLevelIndex++));
    }

    public bool IsShrineComplete()
    {
        return currentLevelIndex == shrineList.GetSize();
    }

    public static LevelManager GetManager()
    {
        if (Instance == null)
        {
            Instance = new GameObject("LevelManager");
            Instance.AddComponent<LevelManager>();
        }
        return Instance.GetComponent<LevelManager>();
    }
}
