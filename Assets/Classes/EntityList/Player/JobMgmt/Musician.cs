using System.Collections.Generic;
using UnityEngine;

public class Musician : JobState
{
    public Musician(Player player) : base(player) { }

    // TODO: implement me
    public override void EnterState(Dictionary<string, object> args = null)
    {
        Rigidbody note = Object.Instantiate(player.MusicNote, player.transform.position + player.cam.transform.forward, Quaternion.identity).GetComponent<Rigidbody>();
        
        note.linearVelocity = player.cam.transform.forward * 30;
    }

    public override void UpdateState()
    {
        player.ExitJobState();
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState(Dictionary<string, object> args = null)
    {

    }
}
