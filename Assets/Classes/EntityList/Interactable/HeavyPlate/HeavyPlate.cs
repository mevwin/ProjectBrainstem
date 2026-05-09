// using System;
// using UnityEngine;

public class HeavyPlate : PressurePlate
{
    public override void Start()
    {
        base.Start();
    }

    protected override void InitializeStates()
    {
        AddState("Unpressed", new HeavyPlateUnpressed(this));
        AddState("Pressed", new HeavyPlatePressed(this));
    }
}
