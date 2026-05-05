using UnityEngine;

public class MultiTriggerDoor : Door
{
    [SerializeField] private ExitTrigger exitTrigger;
    [SerializeField] private int requiredTriggers = 1;
    private int triggers = 0;

    public override void Start()
    {
        base.Start();
    }

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
        isActive = (triggers >= requiredTriggers);
        exitTrigger.isActive = isActive;
        gameObject.transform.position = isActive ? stopPoint.position : startPoint;
    }
}
