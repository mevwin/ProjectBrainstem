using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : Menu
{
    public override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
    }

    protected override void InitializeButtonFunction()
    {
        buttonDict["ResumeButton"].onClick.AddListener(gameManager.TogglePauseMenu);
        buttonDict["MainMenuButton"].onClick.AddListener(gameManager.ReturnToMainMenu);
    }
}
