using UnityEngine;

public class ExitTrigger : Interactable
{
    [SerializeField] private LevelManager levelManager;

    protected override void InitializeStates() { }

    public override void OnTriggerEnter(Collider collider)
    {
        if (isActive && collider.gameObject.TryGetComponent(out Player player))
        {
            Debug.Log("Player Exited Level");
            levelManager.LoadLevel("TestPuzzle1");
        }
    }
}
