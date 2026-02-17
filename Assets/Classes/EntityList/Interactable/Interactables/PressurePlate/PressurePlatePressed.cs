using System.Collections.Generic;
using UnityEngine;

public class PressurePlatePressed : InteractableState
{
    public PressurePlatePressed(PressurePlate plate) : base(plate) { }

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
        interactable.ChangeState("Unpressed");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}
