using UnityEngine;

public class BlueSplotch : Interactable
{
    [SerializeField] private float elevateSpeed = 10f;

    protected override void InitializeStates() { }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player) && !player.IsGrounded())
        {
            Vector3 splotchMovement = player.GetRigidbodyVelocity();

            float dot = Vector3.Dot(transform.up, Vector3.up);
            
            if (dot > 0.99f)
            {
                splotchMovement.y = elevateSpeed;
                player.UpdateMovementVector(splotchMovement, true);
            }
            else
            {
                splotchMovement = transform.up * elevateSpeed;
                // splotchMovement.y = 0f;
                player.splotchMovement = splotchMovement;
                player.ignoreGravity = true;
            }            
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
