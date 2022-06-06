using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog_PatrolState : PatrolState
{
    private Frog frog;
    public Frog_PatrolState(FiniteStateMachine stateMachine, Enemy enemy, D_PatrolState stateData, Frog frog) : base(stateMachine, enemy, stateData)
    {
        this.frog = frog;
    }

    public override void Enter()
    {
        base.Enter();
        roll = Random.Range(1, 100);
        Debug.Log(roll);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        TurnOrStop();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TurnOrStop()
    {
        if (!doMove)
        {
            //turn and move again OR change to idle
            if (roll < 50)
            {
                frog.idleState.SetFlipAfterIdle(true);
                stateMachine.ChangeState(frog.idleState);
            }
            else
                RestartMovement();
        }
    }
}
