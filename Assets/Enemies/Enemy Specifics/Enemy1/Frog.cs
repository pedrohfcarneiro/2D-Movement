using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    public Frog_PatrolState patrolState { get; private set; }
    public Frog_IdleState idleState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_PatrolState patrolStateData;

    public override void Start()
    {
        base.Start();
        {
            patrolState = new Frog_PatrolState(stateMachine, this, patrolStateData, this);
            idleState = new Frog_IdleState(stateMachine, this, idleStateData, this);

            stateMachine.Initialize(patrolState);
        }
    }
}
