using UnityEditor;
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
        {
            if (player.StoredJob != JobManager.Job.NONE)
            {
                string path = $"{JobManager.JobEnumToString(player.StoredJob)}Item";
                Instantiate(Resources.Load(path), transform.position, Quaternion.identity);
            }
            player.SetStoredJob(job);
        }

        isActive = true;
        base.DetectActivation();

        Destroy(gameObject);
    }

    public override void Drop()
    {
        base.Drop();
    }
}
