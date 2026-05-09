using System;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Interactable
{
    public MeshRenderer meshRenderer;
    public GameObject meshParent;
    [NonSerialized] public Color unpressedColor;
    [SerializeField] private float toggleOffset = 2.5f;
    [NonSerialized] public Vector3 pressedPos = Vector3.zero;
    [NonSerialized] public Vector3 unpressedPos = Vector3.zero;

    public List<GameObject> objectsPressed = new();

    public override void Start()
    {
        base.Start();

        SetStartingState("Unpressed");
        unpressedColor = meshRenderer.materials[2].color;
        pressedPos.y = -toggleOffset;
    }

    public override void Update()
    {
        objectsPressed.RemoveAll(item => item == null);
        isActive = objectsPressed.Count > 0;

        if (meshParent.transform.localPosition == pressedPos && !isActive)
        {
            ChangeState("Unpressed");
        }

        DetectActivation();
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
