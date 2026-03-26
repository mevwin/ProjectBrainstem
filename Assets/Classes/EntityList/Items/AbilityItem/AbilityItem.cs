using UnityEngine;

public class AbilityItem : Item
{
    [SerializeField] private JobManager.Job job;

    public override void Pickup(Player player)
    {
        base.Pickup(player);
        Debug.Log("Ability Pickup: " + JobManager.JobEnumToString(job));
        player.SetPlayerJobAbility(job);
        Destroy(gameObject);
    }

    public override void Drop()
    {
        base.Drop();
    }
}
