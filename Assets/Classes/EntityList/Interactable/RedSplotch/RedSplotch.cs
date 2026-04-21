using UnityEngine;

public class RedSplotch : Interactable
{
    private const float MAX_SPEED = 65f;

    protected override void InitializeStates() { }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player) &&
            player.abilityActive && player.CurrentJob == JobManager.Job.ATHLETE &&
            gameObject == player.targetVaultSpot.transform.parent.gameObject
        ) {
            player.initiatePullJump = true;
        }
    }
    
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player) &&
            !player.ignoreGravity
        ) {
            Vector3 playerVelocity = player.GetRigidbodyVelocity();
            
            float speed = Mathf.Min(playerVelocity.magnitude, MAX_SPEED);

            player.splotchMovementDecayRate = speed;

            Vector3 splotchMovement = speed * transform.up;
            player.splotchMovement = splotchMovement;
            player.ignoreGravity = true;
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.ignoreGravity = false;
        }
    }
}
