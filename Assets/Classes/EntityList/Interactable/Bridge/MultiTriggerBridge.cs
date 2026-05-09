using UnityEngine;

public class MultiTriggerBridge : Interactable, ITriggerListener
{
    [SerializeField] private GameObject bridgeObj;
    [SerializeField] private int requiredTriggers = 1;
    private int triggers = 0;

    protected override void InitializeStates() { }

    public virtual void OnTriggerEvent(TriggerEventType eventType)
    {
        if (eventType == TriggerEventType.Activated)
        {
            triggers++;
        }
        else if (eventType == TriggerEventType.Deactivated)
        {
            triggers--;
            if (triggers < 0) triggers = 0;
        }
        isActive = triggers >= requiredTriggers;
        bridgeObj.SetActive(isActive);
    }
}
