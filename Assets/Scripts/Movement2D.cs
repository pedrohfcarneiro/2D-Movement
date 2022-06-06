using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    //Reference to the player's rigidbody
    private Rigidbody2D rb;
    //Reference to the player's sprite renderer
    private SpriteRenderer player;
    //Reference to the player's animator
    private Animator anim;

    //Movement Stuff
    private Vector2 moveInput = new Vector2();
    private Vector2 targetVelocity = new Vector2();
    [Header("Movement")]
    public float normalMovementSpeed = 10f;
    public float normalMovementSmoothness = 0.05f;
    private float movementSmoothness = .05f;
    private float movementSpeed = 10f;
    private bool facingRight;
    private bool canFlip;
    private bool isRunning;
    [Header("Jump")]
    public float jumpVelocity = 4f;
    public float bufferMax = 0.05f;
    public float gravityHolding = 3f;
    public float gravityNotHolding = 5f;
    [Range(1,4f)]public float airRestriction = 0.33f;
    [SerializeField] private Transform groundCheckPoint;	// A position marking where to check if the player is grounded.
    private float groundCheckRadius = .2f;
    private float bufferCounter = 20f;
    private bool isGrounded;
    private bool isSliding;
    private bool isJumping;
    private bool isHolding;
    private bool canJump;
    private bool jumpRequested;
    [Header("Dash")]
    public float dashSpeed = 10f;
    public bool canDash = true;
    public float cooldown = 0.3f;
    public float duraçãoDash = 0.8f;
    private float duraçãoAtual = 0;
    private bool isDashing = false;
    private bool doDash = false;
    private bool dashInCooldown = false;
    [SerializeField] private LayerMask groundLayer;
    private Vector2 velocity = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        jumpRequested = false;
        isJumping = false;
        isDashing = false;
        isRunning = false;
        canFlip = true;
        if (player.flipX == true)
        {
            facingRight = false;
        }
        else facingRight = true;
    }

    
    void Update()
    {
        MyInput();
        Flip();
        AirRestriction();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Movement();
        Dash();
    }


    public void MyInput()
    {
        //Movement inputs
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        //Checks if player is moving
        if (rb.velocity.magnitude > 0.3f)
            isRunning = true;
        else if(rb.velocity.magnitude < 0.3f)
            isRunning = false;

        anim.SetBool("isRunning", isRunning);


        //Checks which direction player is facing
        if (player.flipX == true)
        {
            facingRight = false;
        }
        else facingRight = true;


        //Jump inputs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
            bufferCounter = 0f;
        }
        // jump inputs+ (checks if player is holding space)
        if (Input.GetKey(KeyCode.Space))
        {
            isHolding = true;
        }
        else isHolding = false;

        if (isJumping && isHolding && rb.velocity.y > 0)
        {
            rb.gravityScale = gravityHolding;
        }
        else rb.gravityScale = gravityNotHolding;

        // jump inputs+ (Checks wether player can jump or not)
        if (isGrounded || isSliding)
        {
            isJumping = false;
            canJump = true;
        }
        else
        {
            isJumping = true;
            canJump = false;
        }

        // Dash inputs

        if (Input.GetKeyDown(KeyCode.C) && duraçãoAtual <= duraçãoDash + 0.04f)  // starts the dash
        {
            if(!dashInCooldown)
            {
                doDash = true;
            }

            if(isDashing == false)
            {
                duraçãoAtual = 0;
            }
        }

        if(dashInCooldown)         // checks if dash is in cooldown
        {
            canDash = false;
        }
        else
        {
            canDash = true;
        }

    }

    public void Movement()
    {
        //horizontal movement
        targetVelocity.x = moveInput.x * movementSpeed; // gets the target velocity on x axis
        targetVelocity.y = rb.velocity.y;               // this is to keep the current y velocity
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothness);  // smoothly adds velocity to player in input direction

        //jumping
        if(bufferCounter < bufferMax)
        {
            bufferCounter += 1 * Time.fixedDeltaTime;
            if (canJump && jumpRequested) // checks if can jump
            {
                rb.velocity = new Vector2(rb.velocity.x , 0f ) + Vector2.up * jumpVelocity;
                jumpRequested = false;
            }
        }




    }

    public void AirRestriction()
    {
        if(isJumping)
        {
            movementSmoothness = normalMovementSmoothness * airRestriction;
        }
        else
        {
            movementSmoothness = normalMovementSmoothness;
        }
    }

    public void Dash()
    {
        // Dash
        if (canDash && doDash)
        {
            isDashing = true;
            if (duraçãoAtual < duraçãoDash)
            {
                duraçãoAtual += 1 * Time.fixedDeltaTime;
                if (facingRight)
                {
                    canFlip = false;
                    rb.velocity = Vector2.right * dashSpeed;
                }
                else
                {
                    canFlip = false;
                    rb.velocity = Vector2.left * dashSpeed;
                }

                rb.gravityScale = 0;
            }
            else
            {
                StopDash();
                StartCoroutine("DashCooldown");
            }
        }
    }

    private void Flip()
    {
        if(moveInput.x > 0 && facingRight == false && canFlip)
        {
            player.flipX = false;
        }
        if(moveInput.x < 0 && facingRight == true && canFlip)
        {
            player.flipX = true;
        }
    }

    private void StopDash()
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 3;
        doDash = false;
        canFlip = true;
        isDashing = false;
    }
    IEnumerator DashCooldown()
    {
        dashInCooldown = true;
        yield return new WaitForSeconds(cooldown);
        dashInCooldown = false;
    }


    private void GroundCheck()
    {
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPoint.position, groundCheckRadius, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                isGrounded = true;
        }
    }

    /*private void OnCollisionStay2D(Collision2D collision)
    {
        int layer = collision.gameObject.layer;
        if (groundLayer == (groundLayer | (1 << layer)))  //  returns true if the collision.contact layer is groundLayer
        {
            isGrounded = true;
            Debug.Log(canJump + "pode");
        }
    }*/

    /*private void OnCollisionExit2D(Collision2D collision)
    {
        int layer = collision.gameObject.layer;
        if (groundLayer == (groundLayer | (1 << layer)))  //  returns true if the collision.contact layer is groundLayer
        {
            isGrounded = false;
            Debug.Log(canJump + "não pode");
        }
    }*/
}
