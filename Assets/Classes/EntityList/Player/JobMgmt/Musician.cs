using System.Collections.Generic;
using UnityEngine;

public class Musician : JobState
{
    public enum Instrument
    {
        NONE,
        KEYTAR,
        TRUMPET
    }

    GameObject CurrentNote;
    public static GameObject bridge;

    public Musician(Player player) : base(player)
    {
        CurrentNote = player.ProjectileNote;
    }

    public override void EnterState(Dictionary<string, object> args = null)
    {
        if (bridge != null && CurrentNote == player.BridgeNote)
        {
            Object.Destroy(bridge);
            bridge = null;
        }
        Rigidbody note = Object.Instantiate(CurrentNote, player.transform.position + player.cam.transform.forward, Quaternion.identity).GetComponent<Rigidbody>();
        if (CurrentNote == player.BridgeNote)
        {
            BridgeNote script = note.gameObject.GetComponent<BridgeNote>();
            script.musician = this;
        }
        Vector3 targetPos = player.cam.transform.position + player.cam.transform.forward * 20;
        Vector3 dir = (targetPos - player.transform.position).normalized;

        note.linearVelocity = dir * 30f;

        player.ExitJobState();
    }

    public override void UpdateState() { }

    public override void FixedUpdateState() { }

    public override void ExitState(Dictionary<string, object> args = null) { }

    public void ChangeInstrument()
    {
        if (CurrentNote == player.BridgeNote)
        {
            CurrentNote = player.ProjectileNote;
            player.instrument = Instrument.TRUMPET;
        }
        else
        {
            CurrentNote = player.BridgeNote;
            player.instrument = Instrument.KEYTAR;
        }

        player.model.ToggleMusicianInstrument(player.instrument);
    }
}
