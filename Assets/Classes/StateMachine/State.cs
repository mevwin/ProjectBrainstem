using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Pre-defined action for an entity.</para>
/// <para>NOTE: all classes inheriting this class do not need to override all the functions (i.e. use what's needed)</para>
/// </summary>
public abstract class State
{
    /// <summary>
    /// <para>Startup behavior for the given State.</para>
    /// 
    /// <para>Example uses: initialize variables, reset counters, assertion checks, etc.</para>
    /// </summary>
    /// <param name="args">
    /// <para>Optional parameters for the State structured as a dictionary.</para>
    /// <para>Should be used if the state requires data from other classes</para>
    /// <para>
    /// Example Use: 
    /// EnterState({
    ///     "new_time": 4f,
    ///     "counter_start": 4});
    /// </para>
    /// </param>
    public virtual void EnterState(Dictionary<string, object> args = null) { }

    /// <summary>
    /// Execution of the desired state behavior with respect to frame rate
    /// </summary>
    public virtual void UpdateState()
    {
        float delta = Time.deltaTime;
    }

    /// <summary>
    /// Execution of the desired state behavior with respect to real time intervals
    /// </summary>
    public virtual void FixedUpdateState()
    {
        float delta = Time.fixedDeltaTime;
    }

    /// <summary>
    /// Used for entities with trigger colliders
    /// Called whenever another collider enters a collision with this entity
    /// </summary>
    /// <param name="other"></param>
    public virtual void OnTriggerEnterState(Collider other) { }

    /// <summary>
    /// Used for entities with trigger colliders
    /// Called whenever another collider exits a collision with this entity
    /// </summary>
    /// <param name="other"></param>
    public virtual void OnTriggerExitState(Collider other) { }

    /// <summary>
    /// <para>End behavior for the given State.</para>
    /// 
    /// <para>Example uses: resetting any variables/behaviors, setting up new data points</para>
    /// </summary>
    /// <param name="args">
    /// <para>Optional parameters for the State structured as a dictionary.</para>
    /// <para>Should be used if the state requires data from other classes</para>
    /// <para>
    /// Example Use: 
    /// ExitState({
    ///     "new_time": 4f,
    ///     "counter_start": 4});
    /// </para>
    /// </param>
    public virtual void ExitState(Dictionary<string, object> args = null) { }
}
