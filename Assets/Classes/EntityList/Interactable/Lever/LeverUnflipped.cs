using System.Collections.Generic;
using UnityEngine;

public class LeverUnflipped : InteractableState
{
    public LeverUnflipped(Lever lever) : base(lever) { }


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
        interactable.ChangeState("Flip");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}
