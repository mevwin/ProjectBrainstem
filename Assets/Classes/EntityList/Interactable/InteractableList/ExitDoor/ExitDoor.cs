using UnityEngine;

public class ExitDoor : Door
{
    [SerializeField] private ExitTrigger exitTrigger;

    public override void OnTriggerEvent(TriggerEventType eventType)
    {
        base.OnTriggerEvent(eventType);
        exitTrigger.gameObject.transform.position = startPoint;
        exitTrigger.isActive = isActive;
    }
}
