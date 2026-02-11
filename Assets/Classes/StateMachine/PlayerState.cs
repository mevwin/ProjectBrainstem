using UnityEngine;

public abstract class PlayerState : State
{
    protected readonly Player player;

    protected PlayerState(Player player) { this.player = player; }
}
