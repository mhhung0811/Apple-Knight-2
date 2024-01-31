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
    private bool isGrounded;
    private bool isFlying;
    private bool isWallSliding;
    private bool isDashing;
    private bool canJump;
    private bool canSlidings;
    private bool canMove;
    private bool canFlip;
    private bool canJumpStomp;
    private bool canCheckHitBoxJumpSomp;

    //public int amountOfJump = 1;

    [SerializeField]
    private PlayerData playerData;

    public Transform groundCheck;
    public Transform wallCheck;
    public Transform hitBoxJumpForce;
    public LayerMask whatIsGround;
    public LayerMask whatIsEnemy;

    public GameObject fireBall;
    
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        canJump = true;
        isFacingRight = true;
        amountOfJumpLeft = playerData.amountOfJump;
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
        }
        else if (!isFacingRight && moveInputDirection > 0)
        {
            Flip();
        }
    }

    private void CheckSurroundings()
    {
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, playerData.wallCheckDistance, whatIsGround);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, whatIsGround);

        //Kiểm tra khi chạm dất bằng dậm nhảy
        if(isGrounded && canCheckHitBoxJumpSomp) 
        {
             CheckAttackHitBoxJumpForce();
        }
        //audio landing
        if (!isGrounded)
            isFlying = true;
        if(isFlying && isGrounded)
        {
            AudioManager.Instance.PlaySound("Landing1");
            isFlying = false;
        }
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
                myRb.velocity = new Vector2(playerData.dashSpeed * facingDirection, 0);
                dashTimeLeft -= Time.deltaTime;
            }
            if(dashTimeLeft < 0)
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
            if (Input.GetKey(KeyCode.S))
            {
                canJumpStomp = true;
                JumpStomp();
            }
            else
            {
                Jump();
            }
            
        }

        if(Input.GetKeyUp(KeyCode.S))
        {
            canJumpStomp= false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Time.time >= lastDash + playerData.dashCoolDown)
            {
                AttemptToDash();
            }
        }
    }
    private void AttemptToDash()
    {
        AudioManager.Instance.PlaySound("Jump");
        isDashing = true;
        dashTimeLeft = playerData.dashTime;
        lastDash = Time.time;
    }
    private void ApplyMove()
    {
        if (canMove)
        {
            myRb.velocity = new Vector2(playerData.moveSpeed * moveInputDirection, myRb.velocity.y);
        }

        if (isWallSliding && canSlidings)
        {
            myRb.velocity = new Vector2(myRb.velocity.x, -playerData.wallSlidingSpeed);
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
            if (isFacingRight)
            {
                facingDirection = 1;
            }
            else
            {
                facingDirection = -1;
            }
        }
    }
    
    private void Jump()
    {
        if (canJump && !canJumpStomp)
        {
            AudioManager.Instance.PlaySound("Jump");
            myRb.velocity = new Vector2(myRb.velocity.x, playerData.jumpForce);
            amountOfJumpLeft--;
        }

    }
    
    private void JumpStomp()
    {
        if (canJumpStomp && isFlying)
        {
            myRb.velocity = new Vector2(0, -playerData.jumpStompForce);
            Debug.Log("JumpStomp");
            canCheckHitBoxJumpSomp = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);

        Gizmos.DrawWireSphere(hitBoxJumpForce.position, playerData.HitBoxJumpForce);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + playerData.wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
    private void CheckIfCanJump()
    {
        if(isGrounded && myRb.velocity.y <= 0.1)
        {
            canJump = true;
            amountOfJumpLeft = playerData.amountOfJump;
        }

        if (isTouchingWall)
        {
            canJump = true;
            amountOfJumpLeft = playerData.amountOfJump;
        }

        if (amountOfJumpLeft < 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

    private void CheckAttackHitBoxJumpForce()
    {
        Collider2D[] detectObject = Physics2D.OverlapCircleAll(hitBoxJumpForce.position, playerData.HitBoxJumpForce, whatIsEnemy);
        if(detectObject == null)
        {
            return;
        }
        foreach(Collider2D collider in detectObject)
        {
            collider.transform.SendMessage("IsDamaged", 10f);

            if (collider.gameObject.CompareTag("Interactable Object"))
            {

                collider.gameObject.GetComponent<IInteractable>().InteractOn();
            }
        }
        canCheckHitBoxJumpSomp = false;
    }

    #region UI Button Move
    public void ButtonMoveLeftEnter()
    {
        moveInputDirection = -1;
    }
    public void ButtonMoveRightEnter()
    {
        moveInputDirection = 1;
    }
    public void ButtonMoveUp() {  
        moveInputDirection = 0; 
        canJumpStomp = false;
    }
    public void ButtonDash()
    {
        if (Time.time >= lastDash + playerData.dashCoolDown)
        {
            AttemptToDash();
        }
    }
    public void ButtonJump()
    {
        Jump();
    }
    public void ButtonJumpStomp()
    {
        JumpStomp();
    }
    public void ButtonMoveDownEnter()
    {
        canJumpStomp = true;
    }
    public void ButonFireBall()
    {
        GameObject b = Instantiate(fireBall);
        b.transform.position = this.transform.position;
        Darts fire = b.GetComponent<Darts>();
        fire.SetUp(facingDirection);
    }
    #endregion
}
