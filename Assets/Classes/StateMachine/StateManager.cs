using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private Dictionary<string, State> stateMap = new();
    private State currentState;

    /// <summary>
    /// Add a new state to the stateMap
    /// </summary>
    /// <param name="name">The name of the state</param>
    /// <param name="state">The State instance</param>
    public void AddState(string name, State state)
    {
        stateMap.Add(name, state);
    }

    /// <summary>
    /// Change the currentState to the newState
    /// </summary>
    /// <param name="newState">The state to begin</param>
    /// <param name="args">Optional parameters for the new state</param>
    public void ChangeState(string newState, Dictionary<string, object> args = null)
    {
        if (stateMap.ContainsKey(newState))
        {
            currentState?.ExitState(args);
            currentState = stateMap[newState];
            currentState.EnterState(args);
        }
        else Debug.LogError(string.Format("State does not exist: {0}", newState));
    }

    public void CurrentStateUpdate()
    {
        currentState.UpdateState();
    }

    public void CurrentStateFixedUpdate()
    {
        currentState.FixedUpdateState();
    }

    public void CurrentStateOnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnterState(other);
    }

    public void CurrentStateOnTriggerExit(Collider other)
    {
        currentState.OnTriggerExitState(other);
    }

    public void SetStartingState(int index)
    {
        currentState = stateMap[stateMap.Keys.ToArray()[index]];
    }
}
