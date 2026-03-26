using UnityEngine;

public class Door : Interactable, ITriggerListener
{
    protected Vector3 startPoint;
    [SerializeField] protected Transform stopPoint;

    public override void Start()
    {
        startPoint = gameObject.transform.position;
    }

    protected override void InitializeStates() { }

    public virtual void OnTriggerEvent(TriggerEventType eventType)
    {
        if (eventType == TriggerEventType.Activated)
        {
            gameObject.transform.position = stopPoint.position;
            isActive = true;
        }
        else if (eventType == TriggerEventType.Deactivated)
        {
            gameObject.transform.position = startPoint;
            isActive = false;
        }
    }
}
