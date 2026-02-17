public class Boulder : Interactable
{
    public override void Start()
    {
        base.Start();

        // update the mass of the boulder based on its transform scale
        rigidBody.mass *= gameObject.transform.localScale.x;
    }

    protected override void InitializeStates() { }
}
