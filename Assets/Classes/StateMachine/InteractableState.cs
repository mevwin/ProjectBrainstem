using UnityEngine;

public abstract class InteractableState : State
{
    protected readonly Interactable interactable;

    protected InteractableState(Interactable interactable) { this.interactable = interactable; }

    public override void OnTriggerEnterState(Collider other) { }

    public override void OnTriggerExitState(Collider other) { }
}
