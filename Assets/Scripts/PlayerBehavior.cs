using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D myRb;
    private float moveInputDirection;
    private int amountOfJumpLeft;
    private float lastTimeSlideWall;
    private float dashTimeLeft;
    private float lastDash = -100;

    private bool isFacingRight;
    private bool isTouchingWall;
    private bool isWalkiing;
    private bool isGrounded;
    private bool canJump;
    private bool isWallSliding;
    private bool canSlidings;
    private bool isDashing;
    private bool canMove;
    private bool isDashToWall;

    public int amountOfJump;

    public float moveSpeed = 10;
    public float jumpForce = 16;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlidingSpeed;
    public float dashTime;
    public float dashSpeed;
    public float dashCoolDown;
    public float facingDirection;
    public float heighDashToWallCheck;
    public float widthDashToWallCheck;

    public Transform groundCheck;
    public Transform wallCheck;
    public Transform DashToWallCheck;
    public LayerMask whatIsGround;
    
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        moveSpeed = 10;
        jumpForce = 16;
        canJump = true;
        isFacingRight = true;
        facingDirection = 1;
        amountOfJump = 1;
        amountOfJumpLeft = amountOfJump;
        lastTimeSlideWall = 0;
        canSlidings = true;
        canMove = true;
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

        isDashToWall = Physics2D.OverlapBox(DashToWallCheck.position, new Vector2(widthDashToWallCheck, heighDashToWallCheck), whatIsGround);
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
        if (isDashing)
        {
            if(dashTimeLeft > 0)
            {
                canMove = false;
                canJump = false;
                myRb.velocity = new Vector2(facingDirection*dashSpeed, 0);
                dashTimeLeft -= Time.deltaTime;
            }
            if(dashTimeLeft < 0 || isDashToWall)
            {
                isDashing = false;
                canMove = true;
                canJump = true;
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

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(Time.time >= (lastDash + dashCoolDown))
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
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
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

        Gizmos.DrawLine(new Vector3(DashToWallCheck.position.x,DashToWallCheck.position.y+heighDashToWallCheck,DashToWallCheck.position.z), new Vector3(DashToWallCheck.position.x, DashToWallCheck.position.y - heighDashToWallCheck, DashToWallCheck.position.z));
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
