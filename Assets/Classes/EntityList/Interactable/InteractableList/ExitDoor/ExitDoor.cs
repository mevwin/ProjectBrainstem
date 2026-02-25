using UnityEngine;

public class ExitDoor : Door
{
    [SerializeField] private ExitTrigger exitTrigger;

    public override void OnTriggerEvent(TriggerEventType eventType)
    {
        base.OnTriggerEvent(eventType);
        exitTrigger.gameObject.transform.position = new(startPoint.x - 0.7f, startPoint.y, startPoint.z);
        exitTrigger.isActive = isActive;
    }
}
