using System.Collections.Generic;
using UnityEngine;

public class JobManager : StateManager
    {
    public enum Job
    {
        NONE,
        BUILDER,
        MUSICIAN,
        ATHLETE,
        ARTIST,
    }

    public static string JobEnumToString(Job job)
    {
        string title = job switch {
            Job.ARTIST => "Artist",
            Job.ATHLETE => "Athlete",
            Job.BUILDER => "Builder",
            Job.MUSICIAN => "Musician",
            _ => "None"
        };
        return title;
    }

    public void ExitJobState()
    {
        ChangeState("None");
    }
}
