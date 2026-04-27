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

    public void PrepAbility(Job job, Dictionary<string, object> args = null)
    {
        stateMap.TryGetValue(JobEnumToString(job), out var state);
        Reticle reticle = null;
        bool nextModePressed = false;
        if (args != null)
        {
            if (args.ContainsKey("Reticle"))
                reticle = (Reticle) args["Reticle"];
            if (args.ContainsKey("NextAbilityMode"))
                nextModePressed = (bool) args["NextAbilityMode"];
        }
        
        switch (job)
        {
            case Job.BUILDER:
                if (nextModePressed)
                    (state as Builder).ChangeBlockSize();

                (state as Builder).ProjectBlock();
                break;
            
            case Job.ATHLETE:
                (state as Athlete).ProjectVaultStrength();
                break;
            
            case Job.ARTIST:
                Artist artistState = state as Artist;
                if (nextModePressed)
                    artistState.CycleNextColor();

                Color color = artistState.SplotchTypeToColor();
                reticle.Toggle(true);
                reticle.ChangeColor(color);

                break;
            case Job.MUSICIAN:
                if (nextModePressed)
                    (state as Musician).ChangeInstrument();

                reticle.Toggle(true);
                reticle.ChangeColor(Color.white);

                break;
        }
    }

    public void ExitJobState()
    {
        ChangeState("None");
    }
}
