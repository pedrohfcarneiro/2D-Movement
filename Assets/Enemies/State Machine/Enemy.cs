using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public FiniteStateMachine stateMachine;

    public D_Enemy enemyBaseData;   //Adiciona no inspector

    public int facingDirection { get; private set; }
    public GameObject GO { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheck;
    [SerializeField]
    private Transform playerCheck;

    private Vector2 velocityWorkspace;

    public virtual void Start()
    {
        facingDirection = -1;
        GO = this.gameObject;
        rb = GO.GetComponent<Rigidbody2D>();
        anim = GO.GetComponent<Animator>();

        stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        if(stateMachine.currentState.stage == State.EVENT.UPDATE)
            stateMachine.currentState.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        if (stateMachine.currentState.stage == State.EVENT.UPDATE)
            stateMachine.currentState.PhysicsUpdate();
    }


    public virtual void Move(float speed)
    {
        velocityWorkspace = new Vector2(facingDirection * speed, rb.velocity.y);
        rb.velocity = velocityWorkspace;
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, GO.transform.right, enemyBaseData.wallCheckDistance, enemyBaseData.whatIsGround);
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, enemyBaseData.ledgeCheckDistance, enemyBaseData.whatIsGround);
    }

    public virtual void Flip()
    {
        facingDirection *= -1;
        GO.transform.Rotate(0f, 180f, 0f);
    }
}
