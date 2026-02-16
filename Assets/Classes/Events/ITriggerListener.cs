public interface ITriggerListener
{
    void OnTriggerEvent(TriggerEventType eventType);
}

public enum TriggerEventType
{
    Activated,
    Deactivated
}
