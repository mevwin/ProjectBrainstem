using UnityEngine;

public class Boulder : Interactable
{
    protected override void InitializeStates()
    {
        AddState("Idle", new BoulderIdle(this));
    }
}
