using System.Collections.Generic;
using UnityEngine;

public class TogglePlateFlipped : InteractableState
{
    TogglePlate plate;
    bool isLocked;

    public TogglePlateFlipped(TogglePlate plate, bool locked = false) : base(plate)
    {
        isLocked = locked;
        this.plate = (TogglePlate) plate;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        Material[] materials = plate.meshRenderer.materials;
        materials[2].color = Color.white;
        plate.meshRenderer.materials = materials;

        plate.meshParent.transform.localPosition += Vector3.down * plate.toggleOffset;
    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {

    }

    public override void OnTriggerEnterState(Collider other)
    {
        base.OnTriggerExitState(other);

        if (isLocked) return;

        interactable.isActive = false;
        interactable.DetectActivation();
        interactable.ChangeState("Unflipped");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}
