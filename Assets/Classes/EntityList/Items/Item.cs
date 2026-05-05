using System.Collections;
using UnityEngine;

public class Item : Interactable
{
    private readonly WaitForSeconds _pickupCooldown= new(1.2f);
    private bool pickupCooldownStarted = false;
    protected bool shot;

    public override void Start()
    {
        base.Start();
    }

    protected override void InitializeStates() { }

    public virtual void Pickup(Player player) { }

    public virtual void Drop()
    {
        if (!rigidBody || shot) return;
        rigidBody.linearVelocity = Vector3.zero;
    }

    public virtual void Throw(Player player)
    {
        if (shot) return;

        float force = weight switch
        {
            Weight.LIGHT => 35,
            Weight.HEAVY => player.CurrentJob == JobManager.Job.ATHLETE ? 25 : 0,
            _ => 0
        };

        if (player.IsAbilityPressed())
        {
            player.RemoveItem();
            rigidBody.linearVelocity = force * player.cam.transform.forward;
            shot = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!pickupCooldownStarted && shot)
        {
            pickupCooldownStarted = true;
            StartCoroutine(PickupCooldown());
        }
    }

    private IEnumerator PickupCooldown()
    {
        //Debug.Log("Pickup cooldown started");
        yield return _pickupCooldown;
        shot = false;
        pickupCooldownStarted = false;
        //Debug.Log("Pickup cooldown ended");
    }

}
