using System.Collections.Generic;
using UnityEngine;

public class LightPlateUnpressed : InteractableState
{
    LightPlate plate;

    public LightPlateUnpressed(LightPlate plate) : base(plate)
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

    public override void OnTriggerEnterState(Collider other)
    {
        base.OnTriggerEnterState(other);
        plate.meshParent.transform.localPosition += Vector3.up * plate.toggleOffset;

        Entity ent = other.gameObject.GetComponent<Entity>();

        if (ent == null) return;
        if (ent.weight != Entity.Weight.LIGHT) return;
        Debug.Log(ent.weight);

        interactable.isActive = true;
        interactable.DetectActivation();
        interactable.ChangeState("Pressed");
    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}