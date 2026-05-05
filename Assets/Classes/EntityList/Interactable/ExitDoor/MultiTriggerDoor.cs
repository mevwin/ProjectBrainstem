using UnityEngine;

public class MultiTriggerDoor : ExitDoor
{
    [SerializeField] private int requiredTriggers = 1;
    private int triggers = 0;

    public override void OnTriggerEvent(TriggerEventType eventType)
    {
        if (eventType == TriggerEventType.Activated)
        {
            triggers++;
        }
        else if (eventType == TriggerEventType.Deactivated)
        {
            triggers--;
        }
        Debug.Log(triggers);
        isActive = triggers >= requiredTriggers;
        if (isActive)
            base.OnTriggerEvent(eventType);
    }
}
