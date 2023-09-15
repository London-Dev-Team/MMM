using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Animator animator;

    private Rigidbody2D rb;

    private float moveInput;

    public LayerMask whatIsGround;
    private bool onGround;
    public Transform groundCheck;
    [SerializeField] public float groundCheckWidth;
    [SerializeField] public float groundCheckHeight;
    public Transform leftUpgroundCheck;
    public Transform rightUpgroundCheck;
    [SerializeField] public float upgroundCheckWidth;
    [SerializeField] public float upgroundCheckHeight;

    private float playerSpeed;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;

    [SerializeField] public float jumpSpeed;
    private bool isJumping;
    private float jumpTimeCounter;
    [SerializeField] public float jumpTime;

    [SerializeField] public float verticalRecoverySpeed;
    [SerializeField] public float horizontalRecoverySpeed;

    [SerializeField] float gravityScale;
    [SerializeField] float fallGravityScale;

    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [SerializeField] float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

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

    }


    void Update()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckWidth, whatIsGround);
        //onGround = Physics2D.OverlapBox(groundCheck.position, new Vector2(groundCheckWidth, groundCheckHeight), whatIsGround);
 

        // Coyote Time
        if (onGround)
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
        //if (Input.GetButtonUp("Jump"))
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


    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(groundCheck.position, groundCheckWidth);
        Gizmos.DrawWireCube(leftUpgroundCheck.position, new Vector3(upgroundCheckWidth, upgroundCheckHeight, 1));
        Gizmos.DrawWireCube(rightUpgroundCheck.position, new Vector3(upgroundCheckWidth, upgroundCheckHeight, 1));

    }

    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }
}
