using UnityEngine;

public class TogglePlate : Interactable
{
    [SerializeField] public bool locked;

    public override void Start()
    {
        base.Start();

        SetStartingState("Unflipped");
    }

    protected override void InitializeStates()
    {
        AddState("Unflipped", new TogglePlateUnflipped(this));
        AddState("Flipped", new TogglePlateFlipped(this, locked));
    }

    public override void DetectActivation()
    {
        base.DetectActivation();
    }
}
