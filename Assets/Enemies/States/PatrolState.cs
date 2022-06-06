using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    protected D_PatrolState stateData;

    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool doMove;
    protected float moveStartingTime;
    protected int roll;
    protected Vector2 moveDirection;

    public PatrolState(FiniteStateMachine stateMachine, Enemy enemy, D_PatrolState stateData) : base(stateMachine, enemy)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        isDetectingWall = enemy.CheckWall();
        isDetectingLedge = enemy.CheckLedge();
        moveStartingTime = startTime;
        doMove = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(!isDetectingLedge || isDetectingWall)
        {
            StopMovement();
        }

        if(Time.time >= moveStartingTime + stateData.moveDuration)
        {
            StopMovement();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        isDetectingWall = enemy.CheckWall();
        isDetectingLedge = enemy.CheckLedge();
        if(doMove)
            enemy.Move(stateData.patrolSpeed);
    }

    public void RestartMovement()
    {
        enemy.Flip();
        moveStartingTime = Time.time;
        doMove = true;
        roll = Random.Range(1, 100);
        Debug.Log(roll);
    }

    public void StopMovement()
    {
        doMove = false;
        enemy.Move(0f);
    }

    public virtual void TurnOrStop()
    {
        if (!doMove)
        {
            RestartMovement();
            Debug.Log(roll);
        }
    }
}
