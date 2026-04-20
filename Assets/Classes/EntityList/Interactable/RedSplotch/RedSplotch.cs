using UnityEngine;

public class RedSplotch : Interactable
{
    private const float MAX_SPEED = 65f;
    private const float SPEED_MULTIPLIER = 1.2f;

    protected override void InitializeStates() { }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player) && !player.IsGrounded())
        {
            Vector3 splotchMovement = player.GetRigidbodyVelocity();

            float dot = Vector3.Dot(transform.up, Vector3.up);
            
            splotchMovement = transform.up * MAX_SPEED;
            // splotchMovement.y = 0f;
            player.splotchMovement = splotchMovement;
            player.ignoreGravity = true;
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.ignoreGravity = false;
            player.splotchMovement = Vector3.zero;
        }
    }
}
