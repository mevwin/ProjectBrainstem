using UnityEngine;

public class Door : Interactable, ITriggerListener
{
    private Vector3 startPoint;
    [SerializeField] private Transform stopPoint;

    public override void Start()
    {
        startPoint = gameObject.transform.position;
    }

    protected override void InitializeStates() { }

    public void OnTriggerEvent(TriggerEventType eventType)
    {
        if (eventType == TriggerEventType.Activated)
        {
            gameObject.transform.position = stopPoint.position;
        }
        else if (eventType == TriggerEventType.Deactivated)
        {
            gameObject.transform.position = startPoint;
        }
    }
}
