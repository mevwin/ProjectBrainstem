using System.Collections.Generic;
using UnityEngine;

public class LightPlatePressed : InteractableState
{
    LightPlate plate;

    public LightPlatePressed(LightPlate plate) : base(plate)   
    {
        this.plate = (LightPlate) interactable;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        
    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {
        
    }

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
