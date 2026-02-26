using UnityEngine;

public class ExitTrigger : Interactable
{
    protected override void InitializeStates() { }

    public override void OnTriggerEnter(Collider collider)
    {
        if (isActive && collider.gameObject.TryGetComponent(out Player player))
        {
            Debug.Log("Player Exited Level");
            GameManager.GetManager().LoadGameState(GameManager.GameState.IN_PLAY);
        }
    }
}
