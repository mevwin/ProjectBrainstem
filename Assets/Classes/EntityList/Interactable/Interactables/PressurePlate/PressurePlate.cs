using UnityEngine;

public class PressurePlate : Interactable
{
    protected override void InitializeStates()
    {
        
    }

    public override void Update()
    {
        DetectActivation();
    }

    public override void DetectActivation()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isActive = true;
        }

        base.DetectActivation();
    }
}
