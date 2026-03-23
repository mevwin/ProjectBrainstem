using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("==Entity Components==")]
    //[SerializeField] private Animator animator;
    [SerializeField] protected Rigidbody rigidBody;
    [SerializeField] protected new Collider collider;
    [SerializeField] private StateManager stateManager;
    [SerializeField] private AudioManager audioManager;

    [Header("==Physics Stats==")]
    //[SerializedField] protected float rotationSpeed;
    public float movementSpeed;

    // [Header("==Game Stats==")]
    // public float currentHealth;
    // public float maxHealth;

    public virtual void Awake()
    {
        // handle edge cases for input data here
        // if (currentHealth < 0 || currentHealth > maxHealth) currentHealth = maxHealth;

        if (audioManager != null)
            audioManager.InitializeAudioDictionary();
    }

    public virtual void Start()
    {
        InitializeStates();
    }

    // Overridable Entity Functions
    public virtual void Update()
    {
        if (Time.timeScale == 0f) return;
        stateManager.CurrentStateUpdate();
    }

    public virtual void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;
        stateManager.CurrentStateFixedUpdate();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        stateManager.CurrentStateOnTriggerEnter(other);
    }

    public virtual void OnTriggerExit(Collider other)
    {
        stateManager.CurrentStateOnTriggerExit(other);
    }

    protected abstract void InitializeStates();

    // General Entity Functions
    public void UpdateMovementVector(Vector3 input, bool overrideGravity = false)
    {
        Vector3 output = input;

        if (overrideGravity)
        {
            output.y = input.y;
        }
        else output.y = Mathf.Max(rigidBody.linearVelocity.y, -65f);
        
        rigidBody.linearVelocity = output;
    }

    public Vector3 GetRigidbodyVelocity()
    {
        return rigidBody.linearVelocity;
    }

    public void SetRigidbodyKinematic(bool value)
    {
        rigidBody.isKinematic = value;
    }

    public void SetColliderStaticFriction(float value)
    {
        collider.material.staticFriction = value;
    }

    public void SetColliderDynamicFriction(float value)
    {
        collider.material.dynamicFriction = value;
    }

    public void SetColliderFrictionCombine(PhysicsMaterialCombine toggle)
    {
        collider.material.frictionCombine = toggle;
    }

    // State Manager Wrapper Functions
    protected void AddState(string name, State state)
    {
        stateManager.AddState(name, state);
    }

    public void ChangeState(string newState, Dictionary<string, object> args = null)
    {
        stateManager.ChangeState(newState, args);
    }

    public void SetStartingState(string state)
    {
        stateManager.SetStartingState(state);
    }

    // Audio Manager Wrapper Functions
    public void PlayAudioSource(string name, float startTime = 0.0f)
    {
        audioManager.PlayAudioSource(name);
    }

    public void PauseAudioSource(string name)
    {
        audioManager.PauseAudioSource(name);
    }

    public void UnPauseAudioSource(string name)
    {
        audioManager.UnPauseAudioSource(name);
    }

    public void StopAudioSource(string name)
    {
        audioManager.StopAudioSource(name);
    }
}
