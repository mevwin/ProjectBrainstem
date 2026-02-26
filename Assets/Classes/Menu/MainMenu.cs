using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu
{
    protected override void InitializeButtonFunction()
    {
        buttonDict["StartGameButton"].onClick.AddListener(gameManager.StartGame);
    }
}
