using System.Collections.Generic;
using UnityEngine;

public class PressurePlatePressed : InteractableState
{
    PressurePlate pressurePlate;
    

    public PressurePlatePressed(PressurePlate plate) : base(plate)
    {
        pressurePlate = plate;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        Material[] materials = pressurePlate.meshRenderer.materials;
        materials[2].color = Color.white;
        pressurePlate.meshRenderer.materials = materials;

        pressurePlate.meshParent.transform.localPosition = pressurePlate.pressedPos;
    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState() { }

    public override void OnTriggerEnterState(Collider other)
    {
        base.OnTriggerEnterState(other);
        pressurePlate.objectsPressed.Add(other.gameObject);
    }

    public override void OnTriggerExitState(Collider other)
    {
        base.OnTriggerExitState(other);
        pressurePlate.objectsPressed.Remove(other.gameObject);

        if (pressurePlate.objectsPressed.Count == 0)
            interactable.ChangeState("Unpressed");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}
