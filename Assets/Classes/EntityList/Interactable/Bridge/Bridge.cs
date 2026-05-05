using UnityEngine;

public class Bridge : Interactable, ITriggerListener
{
    [SerializeField] private GameObject bridgeObj;
    protected override void InitializeStates() { }

    public virtual void OnTriggerEvent(TriggerEventType eventType)
    {
        isActive = !isActive;
        bridgeObj.SetActive(isActive);
    }
}
