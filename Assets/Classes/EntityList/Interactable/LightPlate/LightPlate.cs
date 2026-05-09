// using System;
// using UnityEngine;

public class LightPlate : PressurePlate
{
    public override void Start()
    {
        base.Start();
    }

    protected override void InitializeStates()
    {
        AddState("Unpressed", new LightPlateUnpressed(this));
        AddState("Pressed", new LightPlatePressed(this));
    }
}
