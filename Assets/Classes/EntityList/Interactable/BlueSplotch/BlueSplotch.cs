using UnityEngine;

public class BlueSplotch : Interactable
{
    protected override void InitializeStates()
    {
        
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            Vector3 elevatedMovement = player.GetRigidbodyVelocity();
            elevatedMovement.y = 15f;

            player.UpdateMovementVector(elevatedMovement, true);
        }
    }
}
