using UnityEngine;

public class Door : Interactable, ITriggerListener
{
    protected override void InitializeStates() { }

    public virtual void OnTriggerEvent(TriggerEventType eventType)
    {
        if (eventType == TriggerEventType.Activated)
        {
            isActive = true;
        }
        else if (eventType == TriggerEventType.Deactivated)
        {
            isActive = false;
        }
    }
}
