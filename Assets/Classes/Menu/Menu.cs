using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour
{
    [SerializeField] protected List<Button> buttons = new();
    protected Dictionary<string, Button> buttonDict = new();
    protected GameManager gameManager;

    public virtual void Start()
    {
        gameManager = GameManager.GetManager();
        foreach (Button button in buttons)
        {
            buttonDict.Add(button.gameObject.name, button);
        }

        InitializeButtonFunction();
    }

    protected abstract void InitializeButtonFunction();
}
