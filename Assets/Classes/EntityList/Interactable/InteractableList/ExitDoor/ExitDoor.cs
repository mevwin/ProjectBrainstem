using UnityEngine;

public class ExitDoor : Door
{
    [SerializeField] private ExitTrigger exitTrigger;

    public override void Start()
    {
        base.Start();
        if (isActive)
            OnTriggerEvent(TriggerEventType.Activated);
    }

    public override void OnTriggerEvent(TriggerEventType eventType)
    {
        exitTrigger.gameObject.transform.SetParent(null, true);
        base.OnTriggerEvent(eventType);
        exitTrigger.isActive = isActive;
    }
}
