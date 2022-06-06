using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog_IdleState : IdleState
{
    private Frog frog;
    public Frog_IdleState(FiniteStateMachine stateMachine, Enemy enemy, D_IdleState stateData, Frog frog) : base(stateMachine, enemy, stateData)
    {
        this.frog = frog;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Time.time >= startTime + stateData.idleDuration)
        {
            stateMachine.ChangeState(frog.patrolState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
