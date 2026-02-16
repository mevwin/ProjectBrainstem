using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : Entity
{
    [SerializeField] public bool isActive = false;

    [SerializeField] private List<MonoBehaviour> listeners = new List<MonoBehaviour>();

    private List<ITriggerListener> cachedListeners = new List<ITriggerListener>();

    public override void Awake()
    {
        foreach (var mb in listeners)
        {
            if (mb is ITriggerListener listener)
            {
                cachedListeners.Add(listener);
            }
        }
    }

    protected void Notify(TriggerEventType eventType)
    {
        foreach (var listener in cachedListeners)
        {
            listener.OnTriggerEvent(eventType);
        }
    }

    public virtual void DetectActivation()
    {
        if (isActive)
        {
            Activated();
        }
        if (!isActive)
        {
            Deactivated();
        }
    }

    public virtual void Activated(Dictionary<string, object> args = null)
    {
        Notify(TriggerEventType.Activated);
    }

    public virtual void Deactivated(Dictionary<string, object> args = null)
    {
        Notify(TriggerEventType.Deactivated);
    }
}
