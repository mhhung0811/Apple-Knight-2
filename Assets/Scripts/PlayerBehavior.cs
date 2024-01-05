using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D myRb;
    private float moveInputDirection;
    private float dashTimeLeft;
    private float lastTimeSlideWall = 0;
    private float lastDash = -100;

    private int amountOfJumpLeft;
    private int facingDirection;


    private bool isFacingRight;
    private bool isTouchingWall;
    private bool isWalkiing;
    private bool isGrounded;
    private bool isWallSliding;
    private bool isDashing;
    private bool canJump;
    private bool canSlidings;
    private bool canMove;
    private bool canFlip;

    public int amountOfJump = 1;

    public float moveSpeed = 10f;
    public float jumpForce = 16f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlidingSpeed;
    public float dashTime;
    public float dashSpeed;
    public float dashCoolDown;

    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask whatIsGround;
    
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        moveSpeed = 10;
        jumpForce = 16;
        canJump = true;
        isFacingRight = true;
        amountOfJump = 1;
        amountOfJumpLeft = amountOfJump;
        lastTimeSlideWall = 0;
        canSlidings = true;
        canMove = true;
        facingDirection = 1;
        canFlip = true;
    }

    void Update()
    {
        CheckInput();
        CheckMoveDirection();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckDash();
    }

    private void FixedUpdate()
    {
        ApplyMove();
        CheckSurroundings();
    }

    private void CheckMoveDirection()
    {
        if (isFacingRight && moveInputDirection < 0)
        {
            Flip();
            facingDirection = -1;
        }
        else if (!isFacingRight && moveInputDirection > 0)
        {
            Flip();
            facingDirection = 1;
        }

        if(myRb.velocity.x != 0)
        {
            isWalkiing = true;
        }
        else
        {
            isWalkiing = false; 
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }
    private void CheckIfWallSliding()
    {
        if (isTouchingWall && !isGrounded && myRb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else if (isTouchingWall && !isGrounded && myRb.velocity.y > 0)
        {
            lastTimeSlideWall = 0;
            canSlidings = true;
            isWallSliding = false;
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void CheckDash()
    {
        if(isDashing)
        {
            if(dashTimeLeft > 0)
            {
                canMove = false;
                canJump = false;
                canFlip = false;
                myRb.velocity = new Vector2(dashSpeed * facingDirection, 0);
                dashTimeLeft -= Time.deltaTime;
            }
            if(dashTimeLeft < 0 || isTouchingWall)
            {
                isDashing = false;
                canMove=true;
                canJump = true;
                canFlip = true;
            }
        }
    }

    private void CheckInput()
    {
        moveInputDirection = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Time.time >= lastDash + dashCoolDown)
            {
                AttemptToDash();
            }
        }
    }
    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;
    }
    private void ApplyMove()
    {
        if (canMove)
        {
            myRb.velocity = new Vector2(moveSpeed * moveInputDirection, myRb.velocity.y);
        }

        if (isWallSliding && canSlidings)
        {
            myRb.velocity = new Vector2(myRb.velocity.x, -wallSlidingSpeed);
            lastTimeSlideWall += Time.deltaTime;
            if(lastTimeSlideWall > 0.7f)
            {
                canSlidings = false;
            }
        }
    }

    private void Flip()
    {
        if(canFlip)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0, 180, 0);
        }
    }
    private void Jump()
    {
        if (canJump)
        {
            myRb.velocity = new Vector2(myRb.velocity.x, jumpForce);
            amountOfJumpLeft--;
        }

    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
    private void CheckIfCanJump()
    {
        if(isGrounded && myRb.velocity.y <= 0.1)
        {
            amountOfJumpLeft = amountOfJump;
        }

        if (amountOfJumpLeft < 0)
        {
            canJump = false;
        }
        else if (isTouchingWall)
        {
            canJump = true;
            amountOfJumpLeft = amountOfJump;
        }
        else
        {
            canJump = true;
        }
    }
}
