using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Animator animator;

    private Rigidbody2D rb;
    private BoxCollider2D boxCol;
    private float moveInput;

    public LayerMask whatIsGround;

    private bool onLeftWall;
    private bool onRightWall;
    private bool onWall;
    private bool onLeftRecovery;
    private bool onRightRecovery;
    private bool onRecovery;
    private bool canLedgeGrab;
    [SerializeField] public float groundCheckWH;
    public Transform leftUpgroundCheck;
    public Transform rightUpgroundCheck;
    public Transform leftWallCheck;
    public Transform rightWallCheck;

    private float playerSpeed;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;

    [SerializeField] public float jumpSpeed;
    private bool isJumping;
    private float jumpTimeCounter;
    [SerializeField] public float jumpTime;

    private bool isFalling;

    [SerializeField] public float upwardsJumpVerticalRecoverySpeed;
    [SerializeField] public float downwardsJumpVerticalRecoverySpeed;

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

        canLedgeGrab = true;

        // Flip checks go here.

    }


    private void FixedUpdate()
    {


        // Running
        playerSpeed = walkSpeed;
        if (Input.GetButton("Run"))
        {
            playerSpeed = runSpeed;
        }

        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector3(moveInput * playerSpeed, rb.velocity.y, 0);

        // Falling
        isFalling = rb.velocity.y < 0 ? true : false;
        
        // Fall from Ledge




    }


    void Update()
    {

        // Wall Checks
        /*
        if ((Physics2D.OverlapBox(leftWallCheck.position, new Vector2(groundCheckWH, groundCheckWH), whatIsGround)) 
            || (Physics2D.OverlapBox(rightWallCheck.position, new Vector2(groundCheckWH, groundCheckWH), whatIsGround)))
        {
            onWall = true;
        }

        if ((Physics2D.OverlapBox(leftUpgroundCheck.position, new Vector2(groundCheckWH, groundCheckWH), whatIsGround)) 
            || (Physics2D.OverlapBox(rightWallCheck.position, new Vector2(groundCheckWH, groundCheckWH), whatIsGround)))
        {
            onRecovery = true;
        }
        */


        onLeftWall = Physics2D.OverlapBox(leftWallCheck.position, new Vector2(groundCheckWH, groundCheckWH), whatIsGround);
        onRightWall = Physics2D.OverlapBox(rightWallCheck.position, new Vector2(groundCheckWH, groundCheckWH), whatIsGround);
        onLeftRecovery = Physics2D.OverlapBox(leftUpgroundCheck.position, new Vector2(groundCheckWH, groundCheckWH), whatIsGround);
        onRightRecovery = Physics2D.OverlapBox(rightUpgroundCheck.position, new Vector2(groundCheckWH, groundCheckWH), whatIsGround);


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

            rb.velocity = Vector2.up * jumpSpeed;

            jumpBufferCounter = 0f;
            StartCoroutine(JumpCooldown());
        }

        // Variable Jump
        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpSpeed;
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

        // WEIGHTED CURVE
        rb.gravityScale = fallGravityScale; // Fall gravity should be on incase player falls off an edge.
        if (rb.velocity.y > 0 && isJumping)
        {
            rb.gravityScale = gravityScale;
        }

        // Recovery
        if (!OnGround())  // If not no wall, but on corner.
        {

            Debug.Log("CAN NOW LEDGE");

            //if ((onWall && !onRecovery))
            if (((!onLeftWall && onLeftRecovery) || (!onRightWall && onRightRecovery)) && canLedgeGrab)
            {
                Debug.Log("UP RECOVER");
                if (isFalling)
                {
                    rb.velocity = new Vector2(rb.velocity.x * downwardsJumpVerticalRecoverySpeed, rb.velocity.y + downwardsJumpVerticalRecoverySpeed);
                }
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x * downwardsJumpVerticalRecoverySpeed, rb.velocity.y + upwardsJumpVerticalRecoverySpeed);
                }

                StartCoroutine(LedgeCooldown());

            }

        }

    }

    private bool OnGround()
    {

        RaycastHit2D groundHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0f, Vector2.down, groundCheckWH, whatIsGround);
        // Debug.Log(raycastHit.collider);
        return groundHit.collider != null;
    }


    private void OnDrawGizmosSelected()
    {
        if (leftUpgroundCheck == null)
        {
            return;
        }

        //Gizmos.DrawWireSphere(groundCheck.position, groundCheckWidth);
        Gizmos.DrawWireCube(leftWallCheck.position, new Vector3(groundCheckWH, groundCheckWH, 1));
        Gizmos.DrawWireCube(rightWallCheck.position, new Vector3(groundCheckWH, groundCheckWH, 1));
        Gizmos.DrawWireCube(leftUpgroundCheck.position, new Vector3(groundCheckWH, groundCheckWH, 1));
        Gizmos.DrawWireCube(rightUpgroundCheck.position, new Vector3(groundCheckWH, groundCheckWH, 1));

    }

    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(0.4f);
        //isJumping = false;
    }

    private IEnumerator LedgeCooldown()
    {
        canLedgeGrab = false;
        yield return new WaitForSeconds(0.25f);
        Debug.Log("DONE");
        canLedgeGrab = true;

    }
}
