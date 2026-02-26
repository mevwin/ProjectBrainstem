using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu
{
    private GameManager gameManager;

    public override void Start()
    {
        gameManager = GameManager.GetManager();
        base.Start();
    }

    protected override void InitializeButtonFunction()
    {
        buttonDict["StartGame"].onClick.AddListener(gameManager.StartGameButton);
    }
}
