using UnityEngine;

public class Lever : Item
{
    float lockedTimer;

    public override void Start()
    {
        base.Start();
    }

    protected override void InitializeStates()
    {

    }

    public override void Pickup(Player player)
    {
        if (lockedTimer > 0f) return;
        lockedTimer = 3f;
        if (isActive)
        {
            Debug.Log("Deactive");
            isActive = false;
            DetectActivation();
        }
        else
        {
            Debug.Log("Active");
            isActive = true;
            DetectActivation();
        }
        player.RemoveItem();
    }

    public override void Update()
    {
        lockedTimer -= Time.deltaTime;
    }

    public override void DetectActivation()
    {
        base.DetectActivation();
    }
}
