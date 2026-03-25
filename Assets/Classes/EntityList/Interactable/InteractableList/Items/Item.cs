using UnityEngine;

public class Item : Interactable
{
    public override void Start()
    {
        base.Start();
    }

    protected override void InitializeStates() { }

    public virtual void Pickup(Player player)
    {
        
    }

    public virtual void Drop()
    {
        rigidBody.linearVelocity = Vector3.zero;
    }
}
