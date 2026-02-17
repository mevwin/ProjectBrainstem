using UnityEngine;

public class Door : Interactable, ITriggerListener
{
    public override void Start() { }

    protected override void InitializeStates() { }

    public void OnTriggerEvent(TriggerEventType eventType)
    {
        if (eventType == TriggerEventType.Activated)
        {
            gameObject.transform.position += new Vector3(0, 100, 0);
        }
        else if (eventType == TriggerEventType.Deactivated)
        {
            gameObject.transform.position -= new Vector3(0, 100, 0);
        }
    }
}
