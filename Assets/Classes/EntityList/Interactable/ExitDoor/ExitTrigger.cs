using UnityEngine;

public class ExitTrigger : Interactable
{
    [SerializeField] private ShrineLevelList shrineLevelList;
    [SerializeField] protected bool backToMainMenu = false;

    protected override void InitializeStates() { }

    public override void OnTriggerEnter(Collider collider)
    {
        if (isActive && collider.gameObject.TryGetComponent<Player>(out _))
        {
            GameManager gameManager = GameManager.GetManager();

            if (backToMainMenu)
                gameManager.ReturnToMainMenu();
            else
            {
                LevelManager levelManager = LevelManager.GetManager();

                if (shrineLevelList)
                {
                    levelManager.currentLevelIndex = 0;
                    levelManager.SetShrineList(shrineLevelList);
                }

                gameManager.LoadGameState(GameManager.GameState.IN_PUZZLE);
            }
        }
    }
}
