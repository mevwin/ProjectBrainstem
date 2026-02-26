using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour
{
    [SerializeField] protected List<Button> buttons = new();
    protected Dictionary<string, Button> buttonDict = new();

    public virtual void Start()
    {
        foreach (Button button in buttons)
        {
            buttonDict.Add(button.gameObject.name, button);
        }

        InitializeButtonFunction();
    }

    protected abstract void InitializeButtonFunction();
}
