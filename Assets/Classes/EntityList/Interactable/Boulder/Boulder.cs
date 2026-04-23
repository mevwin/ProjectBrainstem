using System;
using UnityEngine;

public class Boulder : Interactable
{
    public float depthToRespawn = 0f;
    private Vector3 startPosition;

    public override void Start()
    {
        base.Start();

        startPosition = transform.position;

        // update the mass of the boulder based on its transform scale
        rigidBody.mass *= gameObject.transform.localScale.x;

    }

    public override void Update()
    {
        base.Update();

        if (transform.position.y < depthToRespawn)
        {
            UpdateMovementVector(Vector3.zero, true);
            transform.position = startPosition;
        }
    }

    protected override void InitializeStates() { }
}
