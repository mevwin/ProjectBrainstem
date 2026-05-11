using UnityEngine;

public class WorldSpaceText : Interactable, ITriggerListener
{
    [SerializeField] private GameObject canvasParent;

    protected override void InitializeStates() { }

    public virtual void OnTriggerEvent(TriggerEventType eventType)
    {
        isActive = eventType == TriggerEventType.Activated;
        canvasParent.SetActive(isActive);
    }
}
