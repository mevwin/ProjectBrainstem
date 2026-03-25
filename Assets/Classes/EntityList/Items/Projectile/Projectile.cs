using UnityEngine;
using static JobManager;

public class Projectile : Item
{
    private bool shot;
    public override void Pickup(Player player)
    {
        if (shot) return;
        base.Pickup(player);
        Vector3 position = player.transform.position + player.cam.transform.forward * 3;
        Vector3 dir = position - transform.position;
        float mag = dir.magnitude;
        mag = Mathf.Clamp(mag, 0f, 10f);
        dir = 10 * mag * dir.normalized;
        rigidBody.linearVelocity = dir;
        rigidBody.angularVelocity *= 0.99f;
        if (player.GetInputAction(Player.InputKey.ABILITY).WasPressedThisFrame())
        {
            player.RemoveItem();
            rigidBody.linearVelocity = player.cam.transform.forward * 100;
            shot = true;
        }
    }

    public override void Drop()
    {
        if (shot) return;
        base.Drop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        shot = false;
    }
}
