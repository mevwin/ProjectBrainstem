using UnityEngine;

public class Door : Interactable, ITriggerListener
{
    protected override void InitializeStates() { }

    public virtual void OnTriggerEvent(TriggerEventType eventType)
    {
        isActive = eventType == TriggerEventType.Activated;
    }
}
