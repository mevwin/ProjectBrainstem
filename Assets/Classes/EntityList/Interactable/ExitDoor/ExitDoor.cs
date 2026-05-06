using UnityEngine;

public class ExitDoor : Door
{
    [SerializeField] protected ExitTrigger exitTrigger;
    [SerializeField] protected ParticleSystem pSystem1;
    [SerializeField] protected ParticleSystem pSystem2;

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

        pSystem1.gameObject.SetActive(isActive);
        pSystem2.gameObject.SetActive(isActive);
    }
}
