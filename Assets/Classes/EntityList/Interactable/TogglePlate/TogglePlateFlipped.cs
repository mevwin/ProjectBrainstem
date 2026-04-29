using System.Collections.Generic;
using UnityEngine;

public class TogglePlateFlipped : InteractableState
{
    bool isLocked;
    public TogglePlateFlipped(TogglePlate plate, bool locked = false) : base(plate)
    {
        isLocked = locked;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {

    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {

    }

    public override void OnTriggerEnterState(Collider other)
    {
        base.OnTriggerExitState(other);

        if (isLocked) return;

        interactable.isActive = false;
        interactable.DetectActivation();
        interactable.ChangeState("Unflipped");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}
