using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class Lever : Item
{
    float lockedTimer;
    private const float posX = 0.45f;
    private const float rotZ = 45f;
    [SerializeField] private float gapTime = 3f;

    [SerializeField] private GameObject model;
    [SerializeField] private MeshRenderer meshRenderer;
    private Color onColor = Color.red;
    private Color offColor;

    public override void Start()
    {
        base.Start();

        offColor = meshRenderer.materials[1].color;
        model.transform.localPosition = new(-posX, 0, 0);
    }

    protected override void InitializeStates() { }

    public override void Pickup(Player player)
    {
        if (lockedTimer > 0f) return;
        lockedTimer = gapTime;

        isActive = !isActive;
        DetectActivation();

        if (player) player.RemoveItem();
    }

    public override void FixedUpdate()
    {
        lockedTimer = Mathf.MoveTowards(lockedTimer, 0f, Time.fixedDeltaTime);
    }

    public override void DetectActivation()
    {
        base.DetectActivation();

        Color targetColor = isActive ? onColor : offColor;
        float targetPosX = isActive ? posX : -posX;
        float targetRotZ = isActive ? rotZ : -rotZ;
        StartCoroutine(ColorChange(targetColor, targetPosX, -targetRotZ));
    }

    private IEnumerator ColorChange(Color targetColor, float posX, float rotZ)
    {
        while(meshRenderer.materials[1].color != targetColor)
        {
            Color color = meshRenderer.materials[1].color;
            color.r = Mathf.MoveTowards(color.r, targetColor.r, Time.fixedDeltaTime);
            color.g = Mathf.MoveTowards(color.g, targetColor.g, Time.fixedDeltaTime);
            color.b = Mathf.MoveTowards(color.b, targetColor.b, Time.fixedDeltaTime);
            meshRenderer.materials[1].color = color;

            model.transform.SetLocalPositionAndRotation(
                Vector3.MoveTowards(model.transform.localPosition, new Vector3(posX, -0.02f, 0f), 1.4f * Time.fixedDeltaTime), 
                Quaternion.RotateTowards(model.transform.localRotation, Quaternion.Euler(0, 0, rotZ), 140f * Time.fixedDeltaTime)
            );
            
            yield return new WaitForFixedUpdate();
        }
    }
}
