using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
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
        ZOOM,
    }

    public static GameObject Instance { get; private set; }

    [Header("==Player Fields==")]
    [SerializeField] private float jumpSpeed = 25f;
    [SerializeField] private float groundDistanceCheck = 0.05f;

    [Header("==Model Fields==")]
    private Vector3 lastPos;
    private float modelIdleTimer = 0f;
    [SerializeField] private Animator HeadAnimator;
    [SerializeField] private Animator BodyAnimator;

    [Header("==Job Fields==")]
    [SerializeField] private JobManager jobManager;
    [SerializeField] private BlockManager blockManager;
    public JobManager.Job CurrentJob { get;  private set; } = JobManager.Job.NONE;
    public JobManager.Job StoredJob { get;  private set; } = JobManager.Job.NONE;
    
    // Athlete
    private RaycastHit[] hits;
    [NonSerialized] public PoleVaultSpot spot;
    [NonSerialized] public bool initiatePullJump = false;
    [NonSerialized] public Vector3 poleVaultBoost = Vector3.zero;
    [NonSerialized] public float poleVaultBoostDecayRate = 7.5f;
    private float vaultDistance = 0f;
    private const float poleMaxDistance = 18f;
    private readonly Vector3 halfExtents = new(1f, 10, 1f);
    private readonly Vector3 boxCastOffset = new(0, 1f, 0);

    // Artist
    public Artist.Splotch CurrentSplotch { get; private set; }= Artist.Splotch.NONE;
    [SerializeField] private GameObject blueSplotchPrefab;
    private BlueSplotch blueSplotch;

    // Private Vars
    readonly Dictionary<InputKey, InputAction> inputActions = new();

    // Player Flags
    bool hasJumped = false;
    [NonSerialized] public bool abilityActive = false;

    // Item Detection
    public GameObject cam;
    [SerializeField] private Transform zoomOffset;
    private Vector3 camOriginalOffset;
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
                switch (CurrentJob)
                {
                    case JobManager.Job.ATHLETE:
                        ZoomDetection();

                        // DebugBoxCast.SimpleDrawBoxCast(
                        //     cam.transform.position + boxCastOffset, 
                        //     halfExtents * 0.5f,
                        //     cam.transform.rotation,
                        //     cam.transform.forward,
                        //     poleMaxDistance,
                        //     Color.red);
                        break;

                    case JobManager.Job.ARTIST:
                        Debug.DrawRay(
                            cam.transform.position + boxCastOffset,
                            cam.transform.forward,
                            Color.red
                        );

                        break;
                }
            }
        }
        else
        {
            cam.transform.localPosition = Vector3.MoveTowards(cam.transform.localPosition, Vector3.zero, Time.deltaTime * 25f);
        }

        // Input Check For Job Abilities
        if (IsAbilityPressed() && IsZoomHeld() && CurrentJob > JobManager.Job.NONE 
            && !abilityActive && itemPresent == null)
        {
            switch (CurrentJob)
            {
                case JobManager.Job.BUILDER:
                    abilityActive = true;
                    jobManager.ChangeState(JobManager.JobEnumToString(CurrentJob));

                    break;

                case JobManager.Job.ATHLETE:
                    if (IsGrounded() && AthleteCheckCollisionsForSpot())
                    {
                        abilityActive = true;
                        jobManager.ChangeState(
                            JobManager.JobEnumToString(CurrentJob),
                            new Dictionary<string, object>()
                            {
                                { "poleDistance", vaultDistance },
                                { "polePosition", spot.transform.position }
                            }
                        );
                    }

                    break;

                case JobManager.Job.ARTIST:
                    if (ArtistCheckForBlueSplotchSpawn())
                    {
                        Debug.Log("blue splotch spawned");
                    }

                    break;

            }
        }

        if (abilityActive && CurrentJob > JobManager.Job.NONE)
            jobManager.CurrentStateUpdate();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Time.timeScale == 0f) return;

        lastPos = transform.position;
        
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

        //Debug.Log(rigidBody.linearVelocity);
    }

    void LateUpdate()
    {
        Vector3 delta = transform.position - lastPos;

        if (delta.magnitude < 1)
            modelIdleTimer += 1f;
        else
            modelIdleTimer++;

        if (modelIdleTimer > 700f)
        {
            HeadAnimator.SetBool("goIdle", true);
            BodyAnimator.SetBool("goIdle", true);

            modelIdleTimer = 0f;

            StartCoroutine(ResetModelState());
        }
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
        }
    }

    private void InitializeJobStates()
    {
        jobManager.AddState("None", new NoJob(this));
        jobManager.AddState("Builder", new Builder(this));
        jobManager.AddState("Athlete", new Athlete(this));

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

        return Physics.SphereCast(bottom, radius, Vector3.down, out RaycastHit hit, maxDistance);
    }

    public bool HasJumped()
    {
        return inputActions[InputKey.JUMP].WasPressedThisFrame() && IsGrounded();
    }

    // Player Model
    IEnumerator ResetModelState()
    {
        yield return new WaitForSeconds(4.1f);
        HeadAnimator.SetBool("goIdle", false);
        BodyAnimator.SetBool("goIdle", false);
    }

    // Job Mgmt
    public bool IsZoomHeld()
    {
        return inputActions[InputKey.ZOOM].IsPressed();
    }

    public bool IsAbilityPressed()
    {
        return inputActions[InputKey.ABILITY].WasPressedThisFrame();
    }

    public void SetCurrentJob(JobManager.Job newJob)
    {
        CurrentJob = newJob;
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

    public void BuilderCreateBlock()
    {
        blockManager.CreateBlock();
    }

    public void BuilderUpdateBlocks()
    {
        blockManager.UpdateBlocks();
    }

    public void ZoomDetection()
    {
        hits = Physics.BoxCastAll(
            cam.transform.position + boxCastOffset, 
            halfExtents * 0.5f,
            cam.transform.forward,
            cam.transform.rotation,
            poleMaxDistance);
    }

    public bool AthleteCheckCollisionsForSpot()
    {
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.TryGetComponent(out spot) && 
                transform.position.y > spot.transform.position.y - 2f &&
                transform.position.y < spot.transform.position.y + 2f
            ) {   
                vaultDistance = hit.distance;
                return true;    
            }
        }
        return false;
    }

    public bool ArtistCheckForBlueSplotchSpawn()
    {
        if (Physics.Raycast(
            cam.transform.position + boxCastOffset, 
            cam.transform.forward, 
            out RaycastHit hit, 
            30f)
        ) {   
            float dot = Vector3.Dot(hit.normal, Vector3.up);
            
            if (dot > 0.99f) { // Closest to 1.0 means flatter
                return true;    // Surface is very flat
            }
        }
        return false;
    }

    public void ArtistSpawnBlueSplotch()
    {
        // blueSplotch = Instantiate(blueSplotchPrefab, ),
    }

    public void ArtistRepositionBlueSplotch()
    {
        
    }

    public void ArtistDespawnBlueSplotch()
    {
        
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
