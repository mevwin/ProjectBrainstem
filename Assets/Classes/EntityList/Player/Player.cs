using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    public enum InputKey
    {
        MOVE,
        JUMP,
        INTERACT,
    }

    [Header("==Player Fields==")]
    [SerializeField] private float jumpSpeed = 25f;
    [SerializeField] private float groundDistanceCheck = 0.05f;

    // Private Vars
    readonly Dictionary<InputKey, InputAction> inputActions = new();

    // Movement Flags
    bool hasJumped = false;


    public override void Start()
    {
        base.Start();

        InitializeInputActionDict();
    }

    public override void Update()
    {
        base.Update();

        if (HasJumped()) {
            // PlayAudioSource("Footsteps");
            hasJumped = true;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        rigidBody.angularVelocity = Vector3.zero;

        if (hasJumped) {
            Vector3 vector = rigidBody.linearVelocity;
            vector.y = jumpSpeed;
            rigidBody.linearVelocity = vector;
            hasJumped = false;
        }

        //Debug.Log(rigidBody.linearVelocity);
    }

    protected override void InitializeStates()
    {
        AddState("Idle", new PlayerIdle(this));
        AddState("Move", new PlayerMove(this));
    }

    private void InitializeInputActionDict()
    {
        if (InputSystem.actions) 
        {
            inputActions.Add(InputKey.MOVE, InputSystem.actions.FindAction("Player/Move"));
            inputActions.Add(InputKey.JUMP, InputSystem.actions.FindAction("Player/Jump"));
            inputActions.Add(InputKey.INTERACT, InputSystem.actions.FindAction("Player/Interact"));
        }
    }

    // Getter Functions
    public Vector3 GetMovementVector()
    {
        InputAction move = GetInputAction(InputKey.MOVE);
        Vector2 inputVector = move.ReadValue<Vector2>();
        Vector3 movementVector = new(inputVector.x, 0, inputVector.y);
        return movementVector;
    }

    public InputAction GetInputAction(InputKey key)
    {
        return inputActions[key];
    }

    // Movement Checks
    public bool IsMoving()
    {
        InputAction move = GetInputAction(InputKey.MOVE);
        return move.ReadValue<Vector2>() != Vector2.zero;
    }

    public bool IsGrounded()
    {
        float radius = (collider as CapsuleCollider).radius;
        float maxDistance = radius + groundDistanceCheck;
        Vector3 bottom = gameObject.transform.position;

        return Physics.SphereCast(bottom, radius, Vector3.down, out RaycastHit hit, maxDistance);
    }

    public bool HasJumped()
    {
        InputAction jump = GetInputAction(InputKey.JUMP);
        return jump.WasPressedThisFrame() && IsGrounded();
    }

    // Debug
    void OnDrawGizmosSelected()
    {
        CapsuleCollider capsuleCollider = collider as CapsuleCollider;
        if (capsuleCollider != null)
        {
            Vector3 capsuleBottom = gameObject.transform.position;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(capsuleBottom + Vector3.down * (capsuleCollider.radius + groundDistanceCheck), capsuleCollider.radius);
        }
    }
}
