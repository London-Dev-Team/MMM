using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Animator animator;

    private Rigidbody2D rb;
    private BoxCollider2D boxCol;

    public LayerMask whatIsGround;

    private float moveInput;
    public int facingDirection = 1;

    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;
    private float playerSpeed;

    [SerializeField] public float jumpSpeed;
    [SerializeField] public float jumpTime;
    private float jumpTimeCounter;
    private bool isJumping;

    private bool canRun;
    private bool isFalling;

    [SerializeField] public float wallCheckWH;
    [SerializeField] public float upwardsLedgeScalar;
    [SerializeField] public float downwardsLedgeScalar;
    [SerializeField] public float downwardsLedgeThreshold;
    private bool onLeftWall;
    private bool onRightWall;
    private bool onLeftLedge;
    private bool onRightLedge;
    private bool canLedgeGrab = true;
    public Transform leftUpgroundCheck;
    public Transform rightUpgroundCheck;
    public Transform leftWallCheck;
    public Transform rightWallCheck;

    [SerializeField] float gravityScale;
    [SerializeField] float fallGravityScale;

    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [SerializeField] float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;


    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();

        playerSpeed = walkSpeed;

    }


    private void FixedUpdate()
    {

        if (moveInput != 0.0f){
            facingDirection = Math.Sign(moveInput);
        }

        // Walk / Run Speeds
        playerSpeed = walkSpeed;
        
        if (OnGround())
        {
            if (Input.GetButton("Run"))
            {
                canRun = true;
                StartCoroutine(RunGradient());
            }
            else
            {
                canRun = false;
            }
        }
        else
        {
            if (canRun)
            {
                playerSpeed = runSpeed;
            }
        }
        

        Debug.Log(moveInput);

        // Horizontal Force
        moveInput = Input.GetAxisRaw("Horizontal");
        
        // rb.velocity = new Vector2(moveInput * playerSpeed, 0);

        if (moveInput <= 1f && moveInput > 0f)
        {
            if (rb.velocity.x < playerSpeed)
            {
                rb.AddForce(new Vector2(moveInput * playerSpeed, 0), ForceMode2D.Impulse);
            }
        }

        if (moveInput >= -1f && moveInput < 0f)
        {
            if (rb.velocity.x > -playerSpeed)
            {
                rb.AddForce(new Vector2(moveInput * playerSpeed, 0), ForceMode2D.Impulse);
            }
        }

        // Clamp-On Max Speed / Stops
        
        if (!(moveInput <= 1f && moveInput > 0f)) 
        {
            if (rb.velocity.x > 0f)
            {
                rb.AddForce(new Vector2(-rb.velocity.x, 0), ForceMode2D.Impulse);
            }
        }

        if (!(moveInput >= -1f && moveInput < 0f))
        {
            if (rb.velocity.x < 0f)
            {
                rb.AddForce(new Vector2(-rb.velocity.x, 0), ForceMode2D.Impulse);
            }
        }
        
        if (rb.velocity.x > playerSpeed)
        {
            rb.AddForce(new Vector2(-(rb.velocity.x - playerSpeed), 0), ForceMode2D.Impulse);
        }

        if (rb.velocity.x < -playerSpeed)
        {
            rb.AddForce(new Vector2(-(rb.velocity.x + playerSpeed), 0), ForceMode2D.Impulse);
        }
        
    }


    void Update()
    {

        onLeftWall = Physics2D.OverlapBox(leftWallCheck.position, new Vector2(wallCheckWH, wallCheckWH), whatIsGround);
        onRightWall = Physics2D.OverlapBox(rightWallCheck.position, new Vector2(wallCheckWH, wallCheckWH), whatIsGround);
        onLeftLedge = Physics2D.OverlapBox(leftUpgroundCheck.position, new Vector2(wallCheckWH, wallCheckWH), whatIsGround);
        onRightLedge = Physics2D.OverlapBox(rightUpgroundCheck.position, new Vector2(wallCheckWH, wallCheckWH), whatIsGround);

        // Falling
        isFalling = rb.velocity.y < 0 ? true : false;

        // Coyote Time
        if (OnGround())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump Buffer
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Normal Jump
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;

            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);

            jumpBufferCounter = 0f;
            StartCoroutine(JumpCooldown());
        }

        // Variable Jump
        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);

                jumpTimeCounter -= Time.deltaTime;

                coyoteTimeCounter = 0f;
            }
            else
            {
                isJumping = false;
            }
        }

        // No Double Jumps
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        // Weighted Jumps 
        rb.gravityScale = fallGravityScale; // Fall gravity should be on incase player falls off an edge.
        if (rb.velocity.y > 0 && isJumping)
        {
            rb.gravityScale = gravityScale;
        }

        // Recovery / Ledge Grab
        if (!OnGround())  // If not no wall, but on corner.
        {

            if (((!onLeftWall && onLeftLedge) || (!onRightWall && onRightLedge)) && canLedgeGrab)
            {

                if (isFalling && rb.velocity.y < -downwardsLedgeThreshold) // Player Falling drastically.
                {
                    rb.velocity = new Vector2(rb.velocity.x * downwardsLedgeScalar, rb.velocity.y + (rb.velocity.y * (-(downwardsLedgeScalar))));
                }
                else if (isFalling) 
                {
                    rb.velocity = new Vector2(rb.velocity.x * downwardsLedgeScalar, rb.velocity.y + (rb.velocity.y * (-(downwardsLedgeScalar)) * downwardsLedgeScalar));
                }
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x * downwardsLedgeScalar, rb.velocity.y + upwardsLedgeScalar); // NOTE: Add Velocity of 1.
                }

                StartCoroutine(LedgeCooldown());

            }

        }

    }

    private bool OnGround()
    {
        RaycastHit2D groundHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0f, Vector2.down, wallCheckWH, whatIsGround);
        return groundHit.collider != null;
    }


    private void OnDrawGizmosSelected()
    {
        if (leftUpgroundCheck == null)
        {
            return;
        }

        //Gizmos.DrawWireSphere(groundCheck.position, groundCheckWidth);
        Gizmos.DrawWireCube(leftWallCheck.position, new Vector3(wallCheckWH, wallCheckWH, 1));
        Gizmos.DrawWireCube(rightWallCheck.position, new Vector3(wallCheckWH, wallCheckWH, 1));
        Gizmos.DrawWireCube(leftUpgroundCheck.position, new Vector3(wallCheckWH, wallCheckWH, 1));
        Gizmos.DrawWireCube(rightUpgroundCheck.position, new Vector3(wallCheckWH, wallCheckWH, 1));
    }
    private IEnumerator RunGradient()
    {
        playerSpeed = walkSpeed + ((runSpeed - walkSpeed) / 2);
        yield return new WaitForSeconds(0.1f);
        playerSpeed = runSpeed;
    }

    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(0.4f);
        // isJumping = false;
    }

    private IEnumerator LedgeCooldown()
    {
        canLedgeGrab = false;
        yield return new WaitForSeconds(0.5f);
        canLedgeGrab = true;
    }
}
