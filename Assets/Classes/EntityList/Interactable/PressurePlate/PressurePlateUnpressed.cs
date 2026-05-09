using System.Collections.Generic;
using UnityEngine;

public class PressurePlateUnpressed : InteractableState
{
    PressurePlate plate;

    public PressurePlateUnpressed(PressurePlate plate) : base(plate)
    {
        this.plate = (PressurePlate) interactable;
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
        base.OnTriggerEnterState(other);

        plate.activeCollider = other.gameObject;
        interactable.isActive = true;
        interactable.DetectActivation();
        interactable.ChangeState("Pressed");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}