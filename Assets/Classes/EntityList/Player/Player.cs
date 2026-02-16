using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    [Header("==Player GameObjects==")]
    [SerializeField] private InputActionAsset inputActions;

    InputAction move;
    InputAction jump;

    public override void Start()
    {
        base.Start();

        if (InputSystem.actions) 
        {
            move = InputSystem.actions.FindAction("Player/Move");
            jump = InputSystem.actions.FindAction("Player/Jump");
        }
    }

    public override void Update()
    {
        base.Update();

        if (HasJumped())
            PlayAudioSource("Footsteps");
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        rigidBody.angularVelocity = Vector3.zero;
    }

    protected override void InitializeStates()
    {
        AddState("Idle", new PlayerIdle(this));
        AddState("Move", new PlayerMove(this));
    }

    public Vector3 GetMovementVector()
    {
        Vector2 inputVector = move.ReadValue<Vector2>();
        Vector3 movementVector = new(inputVector.x, 0, inputVector.y);
        return movementVector;
    }

    public bool IsMoving()
    {
        return move.ReadValue<Vector2>() != Vector2.zero;
    }

    public bool HasJumped()
    {
        return jump.WasPressedThisFrame();
    }
}
