using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D myRb;
    private float moveInputDirection;
    private int amountOfJumpLeft;

    private bool isFacingRight;
    private bool isTouchingWall;
    private bool isWalkiing;
    private bool isGrounded;
    private bool canJump;
    private bool isWallSliding;

    public int amountOfJump;

    public float moveSpeed;
    public float jumpForce;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlidingSpeed;

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
    }

    void Update()
    {
        CheckInput();
        CheckMoveDirection();
        CheckIfCanJump();
        CheckIfWallSliding();
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
        }
        else if (!isFacingRight && moveInputDirection > 0)
        {
            Flip();
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
        else
        {
            isWallSliding = false;
        }
    }

    private void CheckInput()
    {
        moveInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void ApplyMove()
    {
        myRb.velocity = new Vector2(moveSpeed * moveInputDirection, myRb.velocity.y);

        if (isWallSliding)
        {
            if(myRb.velocity.y < wallSlidingSpeed)
            {
                myRb.velocity = new Vector2(myRb.velocity.x, -wallSlidingSpeed);
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
