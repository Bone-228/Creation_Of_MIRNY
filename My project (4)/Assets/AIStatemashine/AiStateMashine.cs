using JetBrains.Annotations;
using UnityEngine;

public class AiStateMashine
{
    public AiState[] states;
    public AiAgent agent;
    public AiStateID currentState;
    public AiStateMashine(AiAgent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(AiStateID)).Length;
        states = new AiState[numStates];

    }

    public void RegisterState(AiState state)
    {
        int index = (int)state.GetID();
        states[index] = state;
    }

    public AiState GetState(AiStateID stateID)
    {
        int index = (int)stateID;
        return states[index];
    }

    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }


    public void ChangeState(AiStateID newState)
    {
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }
}

