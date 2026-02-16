using UnityEngine;

public class PressurePlate : Interactable
{
    protected override void InitializeStates()
    {
        AddState("Unpressed", new PressurePlateUnpressed(this));
        AddState("Pressed", new PressurePlatePressed(this));
    }

    public override void DetectActivation()
    {

        base.DetectActivation();
    }
}
