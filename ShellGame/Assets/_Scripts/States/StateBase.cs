using UnityEngine;

public abstract class StateBase
{

    protected StateMachine StateMachine;

    public StateBase(StateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }

}
