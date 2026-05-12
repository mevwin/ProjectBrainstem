using UnityEngine;

public abstract class JobState : State
{
    protected readonly Player player;

    protected JobState(Player player) { this.player = player; }
}
