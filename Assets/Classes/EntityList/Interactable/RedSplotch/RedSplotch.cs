using UnityEngine;

public class RedSplotch : Interactable
{
    private const float MIN_SPEED = 7f;
    private const float MAX_SPEED = 65f;
    private float velocityEntered = 0f;

    protected override void InitializeStates() { }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player)) {
            player.initiatePullJump = true;
            velocityEntered = Mathf.Max(Mathf.Abs(player.GetRigidbodyVelocity().y), MIN_SPEED);
            Debug.Log(velocityEntered);
        }
    }
    
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player)) {
            // Vector3 playerVelocity = player.GetRigidbodyVelocity();
            
            float speed = Mathf.Min(velocityEntered, MAX_SPEED);

            player.splotchMovementDecayRate = speed;

            Vector3 splotchMovement = speed * transform.up;
            player.splotchMovement = splotchMovement;
            player.ignoreGravity = true;
        }
        else if (other.gameObject.TryGetComponent(out BuilderBlock block))
        {
            float speed = Mathf.Max(block.GetRigidbodyVelocity().magnitude, MIN_SPEED);

            if (block.weight == Weight.HEAVY)
                speed = 0.5f;

            Vector3 output = speed * transform.up;

            block.UpdateMovementVector(output, true);
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.ignoreGravity = false;
            velocityEntered = 0;
        }
    }
}
