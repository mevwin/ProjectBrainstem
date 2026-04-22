using System;
using NUnit.Framework;
using UnityEngine;

public class ExitTrigger : Interactable
{
    [SerializeField] private ShrineLevelList shrineLevelList;

    protected override void InitializeStates() { }

    public override void Start()
    {
        LevelManager levelManager = LevelManager.GetManager();

        if (shrineLevelList)
            levelManager.SetShrineList(shrineLevelList);
    }

    public override void OnTriggerEnter(Collider collider)
    {
        if (isActive && collider.gameObject.TryGetComponent<Player>(out _))
        {
            GameManager gameManager = GameManager.GetManager();
            gameManager.LoadGameState(GameManager.GameState.IN_PUZZLE);
        }
    }
}
