using System;
using UnityEngine;

public class PressurePlate : Interactable
{
    public MeshRenderer meshRenderer;
    public GameObject meshParent;
    [NonSerialized] public Color unpressedColor;
    private const float toggleOffset = 2.5f;
    private Vector3 pressedPos = Vector3.zero;
    private Vector3 unpressedPos = Vector3.zero;
    [NonSerialized] public GameObject activeCollider;

    public override void Start()
    {
        base.Start();

        SetStartingState("Unpressed");
        unpressedColor = meshRenderer.materials[2].color;
        pressedPos.y = -toggleOffset;
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

    public override void FixedUpdate()
    {
        Material[] materials = meshRenderer.materials;

        // Debug.Log(activeCollider);
        if (activeCollider != null && activeCollider.layer != 12)
        {
            materials[2].color = Color.white;
            meshParent.transform.localPosition = pressedPos;
        }
        else
        {
            materials[2].color = unpressedColor;
            meshParent.transform.localPosition = unpressedPos;
            activeCollider = null;
        }


        meshRenderer.materials = materials;
    }
}
