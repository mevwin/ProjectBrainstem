using UnityEngine;

public class BuilderBlock : Item
{
    public override void Pickup(Player player)
    {
        
    }

    public override void Drop()
    {
        
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }
}
