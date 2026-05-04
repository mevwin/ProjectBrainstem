using UnityEngine;

public class MoveableCube : Item
{
    public override void Pickup(Player player)
    {
        base.Pickup(player);
        Vector3 position = player.transform.position + player.cam.transform.forward * 4;
        Vector3 dir = position - transform.position;
        float mag = dir.magnitude;
        mag = Mathf.Clamp(mag, 0f, 10f);
        dir = dir.normalized * mag * 10;
        rigidBody.linearVelocity = dir;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public override void Drop()
    {
        rigidBody.constraints = RigidbodyConstraints.None;
    }
}
