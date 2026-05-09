using System.Collections.Generic;
using UnityEngine;

public class PressurePlateUnpressed : InteractableState
{
    PressurePlate pressurePlate;

    public PressurePlateUnpressed(PressurePlate plate) : base(plate)
    {
        pressurePlate = plate;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        pressurePlate.TogglePlateMaterial(Color.white);
    }

    public override void UpdateState() { }

    public override void FixedUpdateState() { }

    public override void OnTriggerEnterState(Collider other)
    {
        base.OnTriggerEnterState(other);

        pressurePlate.objectsPressed.Add(other.gameObject);
        pressurePlate.isActive = true;
        pressurePlate.DetectActivation();
        pressurePlate.ChangeState("Pressed");
    }

    public override void ExitState(Dictionary<string, object> args = null) { }
}