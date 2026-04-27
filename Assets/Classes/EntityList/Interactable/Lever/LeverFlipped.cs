using System.Collections.Generic;
using UnityEngine;

public class LeverFlipped : InteractableState
{
    public LeverFlipped(Lever lever) : base(lever) { }


    public override void EnterState(Dictionary<string, object> args = null)
    {

    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {

    }

    public override void OnTriggerExitState(Collider other)
    {
        base.OnTriggerExitState(other);

        interactable.isActive = false;
        interactable.DetectActivation();
        interactable.ChangeState("Unflipped");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}
