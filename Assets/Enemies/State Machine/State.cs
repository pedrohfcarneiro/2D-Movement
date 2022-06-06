using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected FiniteStateMachine stateMachine;
    protected Enemy enemy;

    public enum EVENT { ENTER, UPDATE, EXIT};
    public EVENT stage;

    protected float startTime;

    public State(FiniteStateMachine stateMachine, Enemy enemy)
    {
        this.stateMachine = stateMachine;
        this.enemy = enemy;
    }

    public virtual void Enter()
    {
        stage = EVENT.ENTER;
        startTime = Time.time;
        stage = EVENT.UPDATE;
    }

    public virtual void Exit()
    {
        stage = EVENT.EXIT;
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }


}
