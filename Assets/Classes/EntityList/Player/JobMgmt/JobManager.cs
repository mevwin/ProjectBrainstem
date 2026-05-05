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
        bool nextModeHeld = false;
        bool nextModeReleased = false;
        Player player = null;
        if (args != null)
        {
            if (args.ContainsKey("Reticle"))
                reticle = (Reticle) args["Reticle"];
            if (args.ContainsKey("NextAbilityModePressed"))
                nextModePressed = (bool) args["NextAbilityModePressed"];
            if (args.ContainsKey("NextAbilityModeHeld"))
                nextModeHeld = (bool) args["NextAbilityModeHeld"];
            if (args.ContainsKey("NextAbilityModeReleased"))
                nextModeReleased = (bool) args["NextAbilityModeReleased"];
            if (args.ContainsKey("Player"))
                player = (Player) args["Player"];
        }
        
        switch (job)
        {
            case Job.BUILDER:
                if (nextModePressed)
                    (state as Builder).ChangeBlockSize();

                (state as Builder).ProjectBlock();
                break;
            
            case Job.ATHLETE:
                Athlete athleteState = state as Athlete;

                if (nextModeReleased && !player.abilityActive)
                {
                    player.abilityActive = true;
                    player.jobManager.ChangeState(
                        JobEnumToString(player.CurrentJob),
                        new Dictionary<string, object>()
                        {
                            { "athleteMode", Athlete.Mode.SPOT_SPAWN }
                        }
                    );
                    return;
                }

                athleteState.PrepAbility(nextModeHeld);
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
