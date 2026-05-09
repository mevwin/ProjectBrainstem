using System;
using UnityEngine;

public class TogglePlate : Interactable
{
    public MeshRenderer meshRenderer;
    public GameObject meshParent;
    [NonSerialized] public Color unflippedColor;
    [NonSerialized] public float toggleOffset = 2.5f;
    public bool locked;

    public override void Start()
    {
        base.Start();

        SetStartingState("Unflipped");
        unflippedColor = meshRenderer.materials[2].color;
    }

    protected override void InitializeStates()
    {
        AddState("Unflipped", new TogglePlateUnflipped(this));
        AddState("Flipped", new TogglePlateFlipped(this, locked));
    }

    public override void DetectActivation()
    {
        base.DetectActivation();
    }
}
