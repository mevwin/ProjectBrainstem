using UnityEngine;

public class HeavyPlate : Interactable
{
    public override void Start()
    {
        base.Start();

        SetStartingState("Unpressed");
    }

    protected override void InitializeStates()
    {
        AddState("Unpressed", new HeavyPlateUnpressed(this));
        AddState("Pressed", new HeavyPlatePressed(this));
    }

    public override void DetectActivation()
    {
        base.DetectActivation();
    }
}
