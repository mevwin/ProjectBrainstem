using UnityEngine;

public class LightPlate : Interactable
{
    public override void Start()
    {
        base.Start();

        SetStartingState("Unpressed");
    }

    protected override void InitializeStates()
    {
        AddState("Unpressed", new LightPlateUnpressed(this));
        AddState("Pressed", new LightPlatePressed(this));
    }

    public override void DetectActivation()
    {
        base.DetectActivation();
    }
}
