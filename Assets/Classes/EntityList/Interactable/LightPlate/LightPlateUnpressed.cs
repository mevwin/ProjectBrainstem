using System.Collections.Generic;
using UnityEngine;

public class LightPlateUnpressed : InteractableState
{
    LightPlate plate;

    public LightPlateUnpressed(LightPlate plate) : base(plate)
    {
        this.plate = plate;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        plate.TogglePlateMaterial(plate.unpressedColor);
    }

    public override void UpdateState() { }

    public override void FixedUpdateState() { }

    public override void OnTriggerEnterState(Collider other)
    {
        if (plate.IsEntityCorrectWeight(other.gameObject, Entity.Weight.LIGHT))
        {
            plate.objectsPressed.Add(other.gameObject);
            plate.isActive = true;
            plate.DetectActivation();
            plate.ChangeState("Pressed");
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}