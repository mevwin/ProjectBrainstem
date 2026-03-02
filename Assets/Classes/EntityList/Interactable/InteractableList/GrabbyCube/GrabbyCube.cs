using UnityEngine;

public class GrabbyCube : Interactable
{
    public override void Start()
    {
        base.Start();
    }

    protected override void InitializeStates() { }

    public void Grabbed(Vector3 position)
    {
        Vector3 dir = position - transform.position;
        float mag = dir.magnitude;
        mag = Mathf.Clamp(mag, 0f, 10f);
        dir = dir.normalized * mag;
        rigidBody.AddForce(dir * 10);
    }

    public void DisableGrav()
    {
        rigidBody.useGravity = false;
    }

    public void EnableGrav()
    {
        rigidBody.useGravity = true;
    }
}
