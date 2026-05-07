using UnityEngine;

public class PauseMenu : Menu
{
    [SerializeField] private GameObject ControlDiagramParent;
    bool toggle = false;

    public override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
    }

    protected override void InitializeButtonFunction()
    {
        buttonDict["ResumeButton"].onClick.AddListener(gameManager.TogglePauseMenu);
        buttonDict["MainMenuButton"].onClick.AddListener(gameManager.ReturnToMainMenu);
        buttonDict["ReturnToHubButton"].onClick.AddListener(gameManager.ReturnToHubWorld);
        buttonDict["ControlsButton"].onClick.AddListener(ToggleControlsDiagram);
    }

    void ToggleControlsDiagram()
    {
        toggle = !toggle;
        ControlDiagramParent.SetActive(toggle);
    }
}
