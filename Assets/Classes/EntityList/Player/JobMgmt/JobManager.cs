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
        
        ControlsTooltip tooltip = player.playerHud.zoomTooltip;

        if (job > Job.NONE)
        {
            tooltip.ToggleTextbox(0, true);
            tooltip.ToggleTextbox(1, true);
        }

        switch (job)
        {
            case Job.BUILDER:
                tooltip.ToggleTextbox(2, true);
                tooltip.UpdateTextbox(0, "Build Block");
                tooltip.UpdateTextbox(1, "Change Block Size");
                tooltip.UpdateTextbox(2, "Destroy Block (when near)");

                if (nextModePressed)
                    (state as Builder).ChangeBlockSize();

                (state as Builder).ProjectBlock();
                break;
            
            case Job.ATHLETE:
                if (player.itemPresent)
                    tooltip.UpdateTextbox(0, "Throw Held Object");
                else
                    tooltip.UpdateTextbox(0, "Start Pole Vault");
                
                tooltip.UpdateTextbox(1, "Place Pole Vault Spot");

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
                tooltip.UpdateTextbox(1, "Change Splotch Color");

                Artist artistState = state as Artist;

                if (artistState.selectedActiveSplotchHit.collider != null)
                    tooltip.UpdateTextbox(0, "Despawn Splotch");
                else
                   tooltip.UpdateTextbox(0, "Place Splotch");

                if (nextModePressed)
                    artistState.CycleNextColor();

                Color color = artistState.SplotchTypeToColor();
                reticle.Toggle(true);
                reticle.ChangeColor(color);

                break;
            case Job.MUSICIAN:
                tooltip.UpdateTextbox(1, "Change Instrument");

                if (nextModePressed)
                    (state as Musician).ChangeInstrument();

                if (player.instrument == Musician.Instrument.KEYTAR)
                    tooltip.UpdateTextbox(0, "Keytar Ramp");
                else
                   tooltip.UpdateTextbox(0, "Trumpet Toot");

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
