using System.Collections.Generic;
using UnityEngine;

public class TogglePlateUnflipped : InteractableState
{
    TogglePlate plate;

    public TogglePlateUnflipped(TogglePlate plate) : base(plate)
    {
        this.plate = (TogglePlate) interactable;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        Material[] materials = plate.meshRenderer.materials;
        materials[2].color = plate.unflippedColor;
        plate.meshRenderer.materials = materials;

        plate.meshParent.transform.localPosition += Vector3.up * plate.toggleOffset;
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

        interactable.isActive = true;
        interactable.DetectActivation();
        interactable.ChangeState("Flipped");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}