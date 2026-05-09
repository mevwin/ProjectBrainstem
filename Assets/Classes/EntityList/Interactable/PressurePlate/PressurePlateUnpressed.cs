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
        Material[] materials = pressurePlate.meshRenderer.materials;
        materials[2].color = pressurePlate.unpressedColor;
        pressurePlate.meshRenderer.materials = materials;

        pressurePlate.meshParent.transform.localPosition = pressurePlate.unpressedPos;
    }

    public override void UpdateState() { }

    public override void FixedUpdateState() { }

    public override void OnTriggerEnterState(Collider other)
    {
        base.OnTriggerEnterState(other);

        pressurePlate.objectsPressed.Add(other.gameObject);
        interactable.ChangeState("Pressed");
    }

    public override void ExitState(Dictionary<string, object> args = null) { }
}