using System;
using NUnit.Framework;
using UnityEngine;

public class ExitTrigger : Interactable
{
    [SerializeField] private DungeonLevelList dungeonLevelList;

    protected override void InitializeStates() { }

    public override void Start()
    {
        LevelManager levelManager = LevelManager.GetManager();

        if (dungeonLevelList)
            levelManager.SetDungeonList(dungeonLevelList);
    }

    public override void OnTriggerEnter(Collider collider)
    {
        if (isActive && collider.gameObject.TryGetComponent(out Player player))
        {
            GameManager gameManager = GameManager.GetManager();
            gameManager.LoadGameState(GameManager.GameState.IN_PUZZLE);
        }
    }
}
