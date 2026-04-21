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
        NEXT_ABILITY_MODE,
        ZOOM,
    }

    public static GameObject Instance { get; private set; }

    [Header("==Player Fields==")]
    [SerializeField] private float jumpSpeed = 25f;
    [SerializeField] private float groundDistanceCheck = 0.05f;
    [SerializeField] private PlayerModel model;
    public Reticle reticle;
    public AthleteLineRenderer athleteLineRenderer;

    [Header("==Job Fields==")]
    [SerializeField] private JobManager jobManager;
    public JobManager.Job CurrentJob { get;  private set; } = JobManager.Job.NONE;
    public JobManager.Job StoredJob { get;  private set; } = JobManager.Job.NONE;
    [NonSerialized] public bool ignoreGravity = false;

    [Header("Builder")]
    public GameObject BlockProjection;
    public GameObject BlockBuilt;
    [NonSerialized] public GameObject CurrentProjectedBlock;

    [Header("Athlete")]
    [NonSerialized] public PoleVaultSpot spot;
    [NonSerialized] public bool initiatePullJump = false;
    [NonSerialized] public Vector3 poleVaultBoost = Vector3.zero;
    private const float poleVaultBoostDecayRate = 7.5f;

    [Header("Artist")]
    public GameObject blueSplotchPrefab;
    public GameObject redSplotchPrefab;
    public GameObject yellowSplotchPrefab;
    [NonSerialized] public Vector3 splotchMovement;
    [NonSerialized] public float splotchMovementDecayRate = 15f;

    [Header("Musician")]
    public GameObject MusicNote;

    // Private Vars
    readonly Dictionary<InputKey, InputAction> inputActions = new();

    // Player Flags
    bool hasJumped = false;
    [NonSerialized] public bool abilityActive = false;

    // Box Cast Fields
    private readonly Vector3 halfExtents = new(3f, 10, 1f);
    public readonly Vector3 boxCastOffset = new(0, 0.65f, 0);

    // Item Detection
    public GameObject cam;
    [SerializeField] private Transform zoomOffset;
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

        if (HasJumped()) 
        {
            // PlayAudioSource("Footsteps");
            hasJumped = true;
        }

        if (HasGrabbed())
            itemPresent.Pickup(this);

        // Detection
        DetectItem();

        // Zoom-In Effect
        if (IsZoomHeld() && !abilityActive)
        {
            cam.transform.localPosition = Vector3.MoveTowards(cam.transform.localPosition, zoomOffset.localPosition, Time.deltaTime * 25f);

            if (!abilityActive)
            {
                jobManager.PrepAbility(
                    CurrentJob,
                    new Dictionary<string, object>()
                    {
                        { "NextAbilityMode", NextAbilityModeWasPressed() },
                        { "Reticle", reticle }
                    }
                );

                // Debugging Raycast Section
                switch (CurrentJob)
                {
                    // case JobManager.Job.ATHLETE:
                    //     DebugBoxCast.SimpleDrawBoxCast(
                    //         cam.transform.position + boxCastOffset, 
                    //         halfExtents * 0.5f,
                    //         cam.transform.rotation,
                    //         cam.transform.forward,
                    //         20f,
                    //         Color.red);

                    //     break;

                    case JobManager.Job.ARTIST:
                        // Debug.DrawRay(
                        //     cam.transform.position,
                        //     cam.transform.forward * 30f,
                        //     Color.red
                        // );

                        break;
                }
            }
        }
        else
        {
            cam.transform.localPosition = Vector3.MoveTowards(cam.transform.localPosition, Vector3.zero, Time.deltaTime * 25f);

            if (CurrentProjectedBlock)
                Destroy(CurrentProjectedBlock);
            
            reticle.Toggle(false);
            athleteLineRenderer.gameObject.SetActive(false);
        }

        // Input Check For Job Abilities
        if (IsAbilityPressed() && IsZoomHeld() && CurrentJob > JobManager.Job.NONE && !abilityActive)
        {
            abilityActive = true;
            jobManager.ChangeState(JobManager.JobEnumToString(CurrentJob));
        }

        if (abilityActive && CurrentJob > JobManager.Job.NONE)
            jobManager.CurrentStateUpdate();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Time.timeScale == 0f) return;
        
        rigidBody.angularVelocity = Vector3.zero;

        if (hasJumped && !abilityActive) 
        {
            Vector3 vector = rigidBody.linearVelocity;
            vector.y = jumpSpeed;
            rigidBody.linearVelocity = vector;
            hasJumped = false;
        }

        if (abilityActive && CurrentJob > JobManager.Job.NONE)
            jobManager.CurrentStateFixedUpdate();

        // Decrease poleVaultBoost overtime
        if (poleVaultBoost.magnitude > 0)
        {
            poleVaultBoost = Vector3.MoveTowards(poleVaultBoost, Vector3.zero, poleVaultBoostDecayRate * Time.fixedDeltaTime);
        }

        if (splotchMovement.magnitude > 0)
        {
            splotchMovement = Vector3.MoveTowards(splotchMovement, Vector3.zero, splotchMovementDecayRate * Time.fixedDeltaTime);
        }

        //Debug.Log(rigidBody.linearVelocity);
    }

    // Initialization
    protected override void InitializeStates()
    {
        AddState("Idle", new PlayerIdle(this));
        AddState("Move", new PlayerMove(this));
        AddState("NoState", new PlayerNoState(this));

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
            inputActions.Add(InputKey.ZOOM, InputSystem.actions.FindAction("Player/Zoom"));
            inputActions.Add(InputKey.NEXT_ABILITY_MODE, InputSystem.actions.FindAction("Player/Next"));
        }
    }

    private void InitializeJobStates()
    {
        jobManager.AddState("None", new NoJob(this));
        jobManager.AddState("Builder", new Builder(this));
        jobManager.AddState("Athlete", new Athlete(this));
        jobManager.AddState("Artist", new Artist(this));
        jobManager.AddState("Musician", new Musician(this));

        SetCurrentJob(JobManager.Job.NONE);
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

        return Physics.SphereCast(bottom, radius, Vector3.down, out _, maxDistance);
    }

    public bool HasJumped()
    {
        return inputActions[InputKey.JUMP].WasPressedThisFrame() && IsGrounded();
    }

    // Job Mgmt
    public bool IsZoomHeld()
    {
        return inputActions[InputKey.ZOOM].IsPressed();
    }

    public bool ZoomWasReleased()
    {
        return inputActions[InputKey.ZOOM].WasReleasedThisFrame();
    }

    public bool IsAbilityPressed()
    {
        return inputActions[InputKey.ABILITY].WasPressedThisFrame();
    }

    public bool NextAbilityModeWasPressed()
    {
        return inputActions[InputKey.NEXT_ABILITY_MODE].WasPerformedThisFrame();
    }

    public void SetCurrentJob(JobManager.Job newJob)
    {
        if (newJob == JobManager.Job.NONE) return;

        model.JobKitToggle(CurrentJob, false);
        CurrentJob = newJob;

        model.JobKitSwitch();
        model.JobKitToggle(newJob, true);
    }

    public void SetStoredJob(JobManager.Job newJob)
    {
        StoredJob = newJob;
    }

    public void ExitJobState()
    {
        abilityActive = false;
        jobManager.ExitJobState();
    }

    public RaycastHit[] ZoomDetection(float distance)
    {
        return Physics.BoxCastAll(
            cam.transform.position + boxCastOffset, 
            halfExtents * 0.5f,
            cam.transform.forward,
            cam.transform.rotation,
            distance);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.TryGetComponent(out PoleVaultSpot testSpot) && 
            abilityActive && CurrentJob == JobManager.Job.ATHLETE &&
            testSpot == spot
        ) {
            initiatePullJump = true;
        }
    }

    // Interact
    public bool HasGrabbed()
    {
        return inputActions[InputKey.INTERACT].IsPressed() && (itemPresent != null);
    }

    public void DetectItem()
    {
        if (Physics.Raycast(transform.position, cam.transform.forward, out RaycastHit hit))
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
        if (inputActions[InputKey.INTERACT].WasReleasedThisFrame() && itemPresent)
        {
            itemPresent.Drop();
            itemPresent = null;
        }
    }

    public void RemoveItem()
    {
        itemPresent = null;
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