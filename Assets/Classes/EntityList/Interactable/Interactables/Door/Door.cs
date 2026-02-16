using UnityEngine;

public class Door : Interactable, ITriggerListener
{
    protected override void InitializeStates()
    {
        
    }

    public void OnTriggerEvent(TriggerEventType eventType)
    {
        if (eventType == TriggerEventType.Activated)
        {
            this.gameObject.transform.position += new Vector3(0, 1, 0);
        }
        if (eventType == TriggerEventType.Deactivated)
        {
            this.gameObject.transform.position -= new Vector3(0, 1, 0);
        }
    }
}
