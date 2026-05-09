using System.Collections.Generic;
using UnityEngine;

public class TogglePlateUnflipped : InteractableState
{
    TogglePlate plate;

    public TogglePlateUnflipped(TogglePlate plate) : base(plate)
    {
        this.plate = plate;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        plate.TogglePlateMaterial(Color.white);
    }

    public override void UpdateState() { }

    public override void FixedUpdateState() { }

    public override void OnTriggerEnterState(Collider other)
    {
        base.OnTriggerEnterState(other);

        interactable.isActive = true;
        interactable.DetectActivation();
        interactable.ChangeState("Flipped");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}