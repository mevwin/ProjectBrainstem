using System;
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
        ABILITY,
    }

    public static GameObject Instance { get; private set; }

    [Header("==Player Fields==")]
    [SerializeField] private float jumpSpeed = 25f;
    [SerializeField] private float groundDistanceCheck = 0.05f;

    // Jop Mgmt
    [SerializeField] private JobManager jobManager;
    private JobManager.Job currentJob = JobManager.Job.NONE;
    
    // Private Vars
    readonly Dictionary<InputKey, InputAction> inputActions = new();

    // Player Flags
    bool hasJumped = false;
    [NonSerialized] public bool abilityActive = false;

    // Item Detection
    [SerializeField] public GameObject cam;
    Item itemPresent;

    public override void Awake()
    {
        base.Awake();

        if (!Instance)
        {
            Instance = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public override void Start()
    {
        base.Start();

        InitializeInputActionDict();
        InitializeJobStates();
    }

    public override void Update()
    {
        base.Update();
        if (Time.timeScale == 0f) return;

        if (HasJumped()) {
            // PlayAudioSource("Footsteps");
            hasJumped = true;
        }

        if (HasGrabbed())
        {
            itemPresent.Pickup(this);
        }

        DetectItem();

        // Job Ability Logic
        // Input Check
        if (IsAbilityPressed() && currentJob > JobManager.Job.NONE && !abilityActive)
        {
            abilityActive = true;
            jobManager.ChangeState(jobManager.JobEnumToString(currentJob));
        }

        if (abilityActive && currentJob > JobManager.Job.NONE)
        {
            jobManager.CurrentStateUpdate();
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Time.timeScale == 0f) return;
        
        rigidBody.angularVelocity = Vector3.zero;

        if (hasJumped) {
            Vector3 vector = rigidBody.linearVelocity;
            vector.y = jumpSpeed;
            rigidBody.linearVelocity = vector;
            hasJumped = false;
        }

        if (abilityActive && currentJob > JobManager.Job.NONE)
        {
            jobManager.CurrentStateFixedUpdate();
        }

        //Debug.Log(rigidBody.linearVelocity);
    }

    // Initialization
    protected override void InitializeStates()
    {
        AddState("Idle", new PlayerIdle(this));
        AddState("Move", new PlayerMove(this));

        SetStartingState("Idle");
    }

    private void InitializeInputActionDict()
    {
        if (InputSystem.actions) 
        {
            inputActions.Add(InputKey.MOVE, InputSystem.actions.FindAction("Player/Move"));
            inputActions.Add(InputKey.JUMP, InputSystem.actions.FindAction("Player/Jump"));
            inputActions.Add(InputKey.INTERACT, InputSystem.actions.FindAction("Player/Interact"));
            inputActions.Add(InputKey.ABILITY, InputSystem.actions.FindAction("Player/Attack"));
        }
    }

    private void InitializeJobStates()
    {
        jobManager.AddState("None", new NoJob(this));
        jobManager.AddState("Builder", new Builder(this));

        SetPlayerJobAbility(JobManager.Job.BUILDER);
        jobManager.SetStartingState("None");
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
        return inputActions[InputKey.MOVE].ReadValue<Vector2>() != Vector2.zero;
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
        return inputActions[InputKey.JUMP].WasPressedThisFrame() && IsGrounded();
    }

    // Job Mgmt
    public bool IsAbilityPressed()
    {
        return inputActions[InputKey.ABILITY].WasPressedThisFrame();
    }

    public void SetPlayerJobAbility(JobManager.Job newJob)
    {
        currentJob = newJob;
    }

    public void ExitJobState()
    {
        abilityActive = false;
        jobManager.ExitJobState();
    }

    // Interact
    public bool HasGrabbed()
    {
        return inputActions[InputKey.INTERACT].IsPressed() && (itemPresent != null);
    }

    public void DetectItem()
    {
        if (Physics.Raycast(this.transform.position, cam.transform.forward, out RaycastHit hit))
        {
            if (hit.transform.gameObject.GetComponent<Item>() && hit.distance <= 3f)
            {
                itemPresent = hit.transform.gameObject.GetComponent<Item>();
                return;
            }
            else if (!HasGrabbed() && itemPresent != null)
            {
                itemPresent.Drop();
                itemPresent = null;
            }
        }
        else if (!HasGrabbed() && itemPresent != null)
        {
            itemPresent.Drop();
            itemPresent = null;
        }
        InputAction grab = GetInputAction(InputKey.INTERACT);
        if (grab.WasReleasedThisFrame() && itemPresent)
        {
            itemPresent.Drop();
            itemPresent = null;
        }
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
