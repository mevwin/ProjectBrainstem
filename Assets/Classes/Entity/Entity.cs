using System;
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
        stateManager.SetStartingState(0);
    }

    // Overridable Entity Functions
    public virtual void Update()
    {
        stateManager.CurrentStateUpdate();
    }

    public virtual void FixedUpdate()
    {
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
    public void UpdateMovementVector(Vector3 direction)
    {
        Vector3 vector = direction;

        vector.x *= movementSpeed;
        vector.y = Math.Max(rigidBody.linearVelocity.y, -65f);
        vector.z *= movementSpeed;
        
        rigidBody.linearVelocity = vector;
    }

    public Vector3 GetRigidbodyVelocity()
    {
        return rigidBody.linearVelocity;
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
