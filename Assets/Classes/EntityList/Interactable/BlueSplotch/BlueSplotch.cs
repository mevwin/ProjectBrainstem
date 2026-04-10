using UnityEngine;

public class BlueSplotch : Interactable
{
    protected override void InitializeStates()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            Vector3 elevatedMovement = player.GetRigidbodyVelocity();
            elevatedMovement.y = 10f;

            player.UpdateMovementVector(elevatedMovement, true);
        }
    }
}
