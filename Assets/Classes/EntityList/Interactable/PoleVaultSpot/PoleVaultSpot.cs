using UnityEngine;

public class PoleVaultSpot : Interactable
{
    protected override void InitializeStates() { }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player) &&
            player.abilityActive && player.CurrentJob == JobManager.Job.ATHLETE
        ) {
            player.initiatePullJump = true;
        }
    }
}
