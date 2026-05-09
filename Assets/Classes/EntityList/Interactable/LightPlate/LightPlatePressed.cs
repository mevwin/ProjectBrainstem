using System.Collections.Generic;
using UnityEngine;

public class LightPlatePressed : InteractableState
{
    LightPlate plate;

    public LightPlatePressed(LightPlate plate) : base(plate)   
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
            plate.objectsPressed.Add(other.gameObject);
    }

    public override void OnTriggerExitState(Collider other)
    {
        plate.objectsPressed.Remove(other.gameObject);

        if (plate.objectsPressed.Count == 0)
        {
            plate.ChangeState("Unpressed");
            plate.isActive = false;
            plate.DetectActivation();
        }
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}
