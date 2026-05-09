using System.Collections.Generic;
using UnityEngine;

public class HeavyPlatePressed : InteractableState
{
    HeavyPlate plate;

    public HeavyPlatePressed(HeavyPlate plate) : base(plate)
    {
        this.plate = (HeavyPlate) interactable;
    }

    public override void EnterState(Dictionary<string, object> args = null) { }

    public override void UpdateState() { }

    public override void FixedUpdateState() { }

    public override void OnTriggerExitState(Collider other)
    {
        base.OnTriggerExitState(other);
        plate.meshParent.transform.localPosition += Vector3.down * plate.toggleOffset;

        interactable.isActive = false;
        interactable.DetectActivation();
        interactable.ChangeState("Unpressed");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}
