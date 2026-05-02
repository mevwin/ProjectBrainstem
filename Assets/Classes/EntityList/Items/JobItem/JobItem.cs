using UnityEngine;

public class JobItem : Item
{
    [SerializeField] private JobManager.Job job;

    public override void Pickup(Player player)
    {
        base.Pickup(player);
        // Debug.Log("Job Pickup: " + JobManager.JobEnumToString(job));
        if (player.CurrentJob == job || player.StoredJob == job)
            return;

        if (player.CurrentJob == JobManager.Job.NONE)
            player.SetCurrentJob(job);
        else
            player.SetStoredJob(job);

        Destroy(gameObject);
    }

    public override void Drop()
    {
        base.Drop();
    }
}
