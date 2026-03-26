using UnityEngine;

public class PressurePlate : Interactable
{
    public override void Start()
    {
        base.Start();

        SetStartingState("Unpressed");
    }

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
