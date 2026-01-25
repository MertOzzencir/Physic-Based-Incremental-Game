using UnityEngine;

public class ToolStateMachine
{

    private ToolStates currentState;


    public void Initilize(ToolStates state)
    {
        currentState = state;
        Debug.Log(currentState);
        currentState.Enter();
    }

    public void ChangeState(ToolStates nextState)
    {
        currentState.Exit();
        currentState = nextState;
        currentState.Enter();
        Debug.Log(currentState);
    }
    public void Update()
    {
        currentState.Update();
    }
    public void FixedUpdate()
    {
        currentState.FixedUpdate();
    }


}
