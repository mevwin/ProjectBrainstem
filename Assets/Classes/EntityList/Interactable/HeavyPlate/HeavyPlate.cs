using System;
using UnityEngine;

public class HeavyPlate : Interactable
{
    public MeshRenderer meshRenderer;
    public GameObject meshParent;
    [NonSerialized] public float toggleOffset = 2.5f;


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
