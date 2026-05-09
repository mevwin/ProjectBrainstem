using System;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Interactable
{
    public MeshRenderer meshRenderer;
    public GameObject meshParent;
    [NonSerialized] public Color unpressedColor;
    [SerializeField] protected float toggleOffset = 2.5f;
    [NonSerialized] public Vector3 pressedPos = Vector3.zero;
    [NonSerialized] public Vector3 unpressedPos = Vector3.zero;
    [NonSerialized] public List<GameObject> objectsPressed = new();

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
            ChangeState("Unpressed");
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

    public void TogglePlateMaterial(Color pressedColor)
    {
        Material[] materials = meshRenderer.materials;

        materials[2].color = isActive ? pressedColor : unpressedColor;
        meshRenderer.materials = materials;

        meshParent.transform.localPosition = isActive ? pressedPos : unpressedPos;
    }

    public bool IsEntityCorrectWeight(GameObject go, Weight desiredWeight)
    {
        go.TryGetComponent(out Entity ent);
        return ent != null && ent.weight == desiredWeight;
    }
}
