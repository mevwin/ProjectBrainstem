using UnityEngine;

public class BlueSplotch : Interactable
{
    [SerializeField] private float elevateSpeed = 10f;

    protected override void InitializeStates() { }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player) && !player.IsGrounded())
        {
            Vector3 splotchMovement = transform.up * elevateSpeed;
            player.splotchMovement = splotchMovement;
            player.ignoreGravity = true;
        }
        else if (other.gameObject.TryGetComponent(out Interactable interactable))
        {
            Vector3 splotchMovement = transform.up * elevateSpeed;
            interactable.UpdateSplotchMovement(splotchMovement);
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
            player.ignoreGravity = false;
    }
}
