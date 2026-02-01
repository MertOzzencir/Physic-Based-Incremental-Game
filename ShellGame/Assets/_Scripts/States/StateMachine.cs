using UnityEngine;

public class StateMachine
{

    private ToolState currentState;


    public void Initilize(ToolState state)
    {
        currentState = state;
        Debug.Log(currentState);
        currentState.Enter();
    }

    public void ChangeState(ToolState nextState)
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
