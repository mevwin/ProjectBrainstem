using System;
using UnityEngine;

public class TogglePlate : Interactable
{
    public MeshRenderer meshRenderer;
    public GameObject meshParent;
    [NonSerialized] public Color unflippedColor;
    [NonSerialized] public float toggleOffset = 2.5f;
    [NonSerialized] public Vector3 flippedPos = Vector3.zero;
    [NonSerialized] public Vector3 unflippedPos = Vector3.zero;
    public bool permaLock;

    public override void Start()
    {
        base.Start();

        SetStartingState("Unflipped");
        unflippedColor = meshRenderer.materials[2].color;
        flippedPos.y = -toggleOffset;
    }

    protected override void InitializeStates()
    {
        AddState("Unflipped", new TogglePlateUnflipped(this));
        AddState("Flipped", new TogglePlateFlipped(this, permaLock));
    }

    public override void DetectActivation()
    {
        base.DetectActivation();
    }

    public void TogglePlateMaterial(Color flippedColor)
    {
        Material[] materials = meshRenderer.materials;

        materials[2].color = isActive ? flippedColor : unflippedColor;
        meshRenderer.materials = materials;

        meshParent.transform.localPosition = isActive ? flippedPos : unflippedPos;
    }
}
