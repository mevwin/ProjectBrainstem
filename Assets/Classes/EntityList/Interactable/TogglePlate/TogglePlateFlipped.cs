using System.Collections.Generic;
using UnityEngine;

public class TogglePlateFlipped : InteractableState
{
    TogglePlate plate;
    bool permaLock;

    public TogglePlateFlipped(TogglePlate plate, bool locked = false) : base(plate)
    {
        permaLock = locked;
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
        base.OnTriggerExitState(other);

        if (permaLock) return;

        interactable.isActive = false;
        interactable.DetectActivation();
        interactable.ChangeState("Unflipped");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}
