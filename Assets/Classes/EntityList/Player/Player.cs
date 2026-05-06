using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    private readonly WaitForSeconds _jobSwitchCooldown = new(0.5f);

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
    [SerializeField] private float jumpSpeed = 30f;
    [SerializeField] private float groundDistanceCheck = 0.05f;
    public PlayerModel model;
    public PlayerHud playerHud;
    public GameObject cam;
    [SerializeField] private Transform zoomOffset;

    [Header("==Job Fields==")]
    public JobManager jobManager;
    public JobManager.Job CurrentJob { get;  private set; } = JobManager.Job.NONE;
    public JobManager.Job StoredJob { get;  private set; } = JobManager.Job.NONE;

    [Header("Builder")]
    public GameObject BlockProjection;
    public GameObject BlockBuilt;
    [NonSerialized] public GameObject CurrentProjectedBlock;

    [Header("Athlete")]
    [NonSerialized] public bool initiatePullJump = false;
    [NonSerialized] public Vector3 poleVaultBoost = Vector3.zero;
    [NonSerialized] public float poleVaultBoostDecayRate = 7.5f;
    [NonSerialized] public GameObject targetVaultSpot;
    public AthleteLineRenderer athleteLineRenderer;
    public GameObject vaultSpotPrefab;
    public GameObject vaultSpotProjection;
    [NonSerialized] public GameObject CurrentProjectedSpot;
    public float athleteSpeedBoost = 1.35f;
    public LayerMask athleteCastMask;

    [Header("Artist")]
    public GameObject blueSplotchPrefab;
    public GameObject redSplotchPrefab;
    [NonSerialized] public Vector3 splotchMovement;
    [NonSerialized] public float splotchMovementDecayRate = 15f;
    public LayerMask artistSpawnCastMask;
    public LayerMask artistDeleteCastMask;

    [Header("Musician")]
    [NonSerialized] public Musician.Instrument instrument = Musician.Instrument.TRUMPET;
    public GameObject BridgeNote;
    public GameObject ProjectileNote;

    // Private Vars
    readonly Dictionary<InputKey, InputAction> inputActions = new();

    // Player Flags
    bool hasJumped = false;
    [NonSerialized] public bool abilityActive = false;
    [NonSerialized] public bool switchCooldownStarted = false;

    // Box Cast Fields
    private readonly Vector3 halfExtents = new(3f, 10, 1f);
    public readonly Vector3 boxCastOffset = new(0, 0.65f, 0);

    // Item Detection
    [NonSerialized] public Item itemPresent;

    public bool inMenu = false;

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

        playerHud.reticle.Toggle(false);
        athleteLineRenderer.gameObject.SetActive(false);
    }

    public override void Update()
    {
        if (Time.timeScale == 0f || inMenu) return;
        base.Update();
        
        if (HasJumped()) 
        {
            // PlayAudioSource("Footsteps");
            hasJumped = true;
        }

        if (HasGrabbed())
            itemPresent.Pickup(this);

        // Detection
        DetectItem();

        // TEMP DELETE LATER
        //if (Input.GetKeyDown("k"))
        //{
        //    Vector3 midpoint = (startPosition + transform.position) / 2;
        //    float length = (startPosition - transform.position).magnitude;
        //    GameObject bridgeObject = Object.Instantiate(bridge, midpoint, Quaternion.identity);
        //    bridgeObject.transform.localScale = new Vector3(bridge.transform.localScale.x, bridge.transform.localScale.y, length);
        //    float yRot = Mathf.Asin((transform.position.x - startPosition.x) / Mathf.Sqrt(Mathf.Pow(transform.position.x - startPosition.x, 2) + Mathf.Pow(transform.position.z - startPosition.z, 2))) * Mathf.Rad2Deg * Mathf.Sign(transform.position.z - startPosition.z);
        //    float xRot = Mathf.Asin((transform.position.y - startPosition.y) / Mathf.Sqrt(Mathf.Pow(transform.position.y - startPosition.y, 2) + Mathf.Pow(transform.position.z - startPosition.z, 2))) * Mathf.Rad2Deg * Mathf.Sign(transform.position.z - startPosition.z) * -1;
        //    bridgeObject.transform.eulerAngles = new Vector3(xRot, yRot, 0f);
        //}

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
                        { "NextAbilityModePressed", NextAbilityModeWasPressed() },
                        { "NextAbilityModeHeld", NextAbilityModeHeld() },
                        { "NextAbilityModeReleased", NextAbilityModeWasReleased() },
                        { "Reticle", playerHud.reticle },
                        { "Player", this }
                    }
                );
            }
        }
        else
        {
            cam.transform.localPosition = Vector3.MoveTowards(cam.transform.localPosition, Vector3.zero, Time.deltaTime * 25f);

            if (ZoomWasReleased())
            {
                if (CurrentProjectedBlock)
                    Destroy(CurrentProjectedBlock);

                if (CurrentProjectedSpot)
                    Destroy(CurrentProjectedSpot);
            
                playerHud.reticle.Toggle(false);
                athleteLineRenderer.gameObject.SetActive(false);
            }
        }

        // Input Check For Job Abilities
        if (SwitchJobWasPressed())
        {
            JobManager.Job storedJob = StoredJob;
            SetStoredJob(CurrentJob);
            SetCurrentJob(storedJob);
        }

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
        if (Time.timeScale == 0f || inMenu) return;
        base.FixedUpdate();
        
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
            poleVaultBoost = Vector3.MoveTowards(poleVaultBoost, Vector3.zero, poleVaultBoostDecayRate * Time.fixedDeltaTime);

        if (splotchMovement.magnitude > 0)
            splotchMovement = Vector3.MoveTowards(splotchMovement, Vector3.zero, splotchMovementDecayRate * Time.fixedDeltaTime);

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
        if (inputActions[InputKey.JUMP].WasPressedThisFrame())
        {
            if (CurrentJob == JobManager.Job.ATHLETE && poleVaultBoost.magnitude > 0)
            {
                poleVaultBoost = Vector3.zero;
                return true;
            }
            else return IsGrounded();
        }
        return false;
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

    public bool SwitchJobWasPressed()
    {
        return  !IsZoomHeld() &&
                !abilityActive &&
                StoredJob > JobManager.Job.NONE &&
                inputActions[InputKey.NEXT_ABILITY_MODE].WasPerformedThisFrame();
    }

    public bool NextAbilityModeWasPressed()
    {
        return inputActions[InputKey.NEXT_ABILITY_MODE].WasPerformedThisFrame();
    }

    public bool NextAbilityModeHeld()
    {
        return inputActions[InputKey.NEXT_ABILITY_MODE].IsPressed();
    }

    public bool NextAbilityModeWasReleased()
    {
        return inputActions[InputKey.NEXT_ABILITY_MODE].WasReleasedThisFrame();
    }

    public void SetCurrentJob(JobManager.Job newJob)
    {
        model.JobKitToggle(CurrentJob, false);
        model.JobKitSwitch(); 
        CurrentJob = newJob;
        
        if (newJob == JobManager.Job.NONE)
            return;

        playerHud.iconManager1.SetIcon(newJob);

        model.JobKitToggle(newJob, true, instrument);
    }

    public void SetStoredJob(JobManager.Job newJob)
    {
        StoredJob = newJob;
        playerHud.iconManager2.SetIcon(newJob);
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

    public IEnumerator JobSwitchCooldown()
    {
        yield return _jobSwitchCooldown;
        switchCooldownStarted = false;
    }

    // Interact
    public bool HasGrabbed()
    {
        return inputActions[InputKey.INTERACT].IsPressed() && (itemPresent != null);
    }

    public void DetectItem()
    {
        if (Physics.Raycast(transform.position, Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized, out RaycastHit hit))
        {
            Item item = hit.transform.gameObject.GetComponent<Item>();
            if (item && hit.distance <= 3f && (item.weight == Entity.Weight.LIGHT || CurrentJob == JobManager.Job.ATHLETE || (CurrentJob == JobManager.Job.BUILDER && IsZoomHeld())))
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