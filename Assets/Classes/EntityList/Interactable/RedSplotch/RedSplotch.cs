using UnityEngine;

public class RedSplotch : Interactable
{
    private const float MAX_SPEED = 65f;
    private const float SPEED_MULTIPLIER = 1.2f;

    protected override void InitializeStates() { }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player) && !player.IsGrounded())
        {
            float newSpeed = Mathf.Clamp(
                player.GetRigidbodyVelocity().magnitude * SPEED_MULTIPLIER,
                0f,
                MAX_SPEED
            );

            Vector3 output = newSpeed * transform.up;
            player.splotchMovement = output;
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player) && !player.IsGrounded())
        {
            player.splotchMovement = Vector3.zero;
        }
    }
}
