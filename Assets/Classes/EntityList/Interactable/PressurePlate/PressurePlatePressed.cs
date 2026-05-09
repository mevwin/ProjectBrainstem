using System.Collections.Generic;
using UnityEngine;

public class PressurePlatePressed : InteractableState
{
    PressurePlate plate;
    

    public PressurePlatePressed(PressurePlate plate) : base(plate)
    {
        this.plate = (PressurePlate) interactable;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {

    }

    public override void UpdateState() { }

    public override void FixedUpdateState() { }

    public override void OnTriggerExitState(Collider other)
    {
        base.OnTriggerExitState(other);

        Debug.Log(other.gameObject.name);

        plate.activeCollider = null;
        interactable.isActive = false;
        interactable.DetectActivation();
        interactable.ChangeState("Unpressed");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}
