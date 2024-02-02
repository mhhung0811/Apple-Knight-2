using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy4 : BaseEnemy
{
    [SerializeField]
    private EnemyData enemyData;

    private Animator anim;
    private Rigidbody2D myRb;
    private bool isDetectPlayerLeft;
    private bool isDetectPleyerRight;

    private float attackCoolDown;
    private float attackTimeLeft;
    private float attackRadius;
    private float HP;
    private float checkGroundDistance = 0.5f;
    private float checkWallDistance = 0.5f;
    private float groundCheckRadius = 0.4f;

    private bool isAttack;
    private bool isGroundedFlip;
    private bool isWallFlip;
    private bool isGround;

    public Transform detectPlayer;
    public Transform attackHitBoxPos;
    public Transform groundCheckFlip;
    public Transform wallCheckFlip;
    public Transform groundCheck;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsGround;

    void Start()
    {
        InitializedEnemy();
    }
    public override void InitializedEnemy()
    {
        enemyData = new EnemyData();

        HP = enemyData.maxHP;
        anim = GetComponent<Animator>();
        myRb = GetComponent<Rigidbody2D>();
        isDetectPlayerLeft = isDetectPleyerRight = false;
        canMove = true;
        attackCoolDown = 1f;
        attackTimeLeft = 1f;
        attackRadius = 1.5f;
        facingDirection = 1;
    }
    void Update()
    {
        if (GameManager.Instance.PauseGame())
        {
            return;
        }
        DetectPlayer();
        CheckMoveDirection();
        CheckGroundOrWall();
        DealDamage();
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.PauseGame())
        {
            return;
        }
        Move();
        MoveAuto();
    }

    private void CheckGroundOrWall()
    {
        isGroundedFlip = Physics2D.Raycast(groundCheckFlip.transform.position, -transform.up, checkGroundDistance, whatIsGround);
        isWallFlip = Physics2D.Raycast(wallCheckFlip.transform.position, transform.right, checkWallDistance, whatIsGround);
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }
    public override void DealDamage()
    {
        isDetectInHitBox = Physics2D.Raycast(this.transform.position, transform.right, 1.5f, whatIsPlayer);
        if (isDetectInHitBox)
        {
            isAttack = true;
        }
        if (isAttack)
        {
            if (attackTimeLeft > 0)
            {
                attackTimeLeft -= Time.deltaTime;
                canMove = false;
            }
            else
            {
                anim.SetBool("isAttack", true);
                CheckAttackHitBox();
                attackTimeLeft = attackCoolDown;
                isAttack = false;
            }
        }
        else
        {
            attackTimeLeft = attackCoolDown;
            canMove = true;
        }
    }
    public void CheckAttackHitBox()
    {
        Collider2D coll = Physics2D.OverlapCircle(attackHitBoxPos.position, attackRadius, whatIsPlayer);
        if (coll != null)
        {
            GameObject player = coll.gameObject;
            player.GetComponent<PlayerCombatController>().TakeDamage(enemyData.damage, this.gameObject, 0);
        }
    }

    public void FinishAttack1()
    {
        anim.SetBool("isAttack", false);
        canMove = true;
        attackTimeLeft = attackCoolDown;
    }
    public void CheckMoveDirection()
    {
        if (isDetectPlayerLeft)
        {
            Flip();
        }

        if (!isDetectPleyerRight && !isDetectPlayerLeft)
        {
            if((isWallFlip || !isGroundedFlip) && isGround)
            {
                FlipAuto();
            }
        }
    }
    public void Flip()
    {
        if (isDetectPlayerLeft)
        {
            transform.Rotate(0, 180, 0);
            if (isFacingLeft)
            {
                facingDirection = -1;
            }
            else
            {
                facingDirection = 1;
            }
        }
    }
    private void MoveAuto()
    {
        if (!isDetectPleyerRight && isGround)
        {
            myRb.velocity = new Vector2(enemyData.speed*facingDirection, 0);
        }
    }
    private void FlipAuto()
    {
        transform.Rotate(0, 180, 0);
        facingDirection = -facingDirection;
    }
    public override void Move()
    {
        if (isDetectPleyerRight && canMove)
        {
            myRb.velocity = new Vector2((enemyData.speed+2) * facingDirection, myRb.velocity.y);
        }
        else
        {
            myRb.velocity = new Vector2(enemyData.speed * 0, myRb.velocity.y);
        }
    }
    public override void DetectPlayer()
    {
        isDetectPlayerLeft = Physics2D.Raycast(detectPlayer.position + transform.right.normalized * -1, transform.right * -1, enemyData.detectionRange - 1, whatIsPlayer);
        isDetectPleyerRight = Physics2D.Raycast(detectPlayer.position, transform.right, enemyData.detectionRange, whatIsPlayer);
        isFacingLeft = Physics2D.Raycast(new Vector2(detectPlayer.position.x - enemyData.detectionRange, detectPlayer.position.y), transform.right, enemyData.detectionRange, whatIsPlayer);
    }
    public override void IsDamaged(float damage)
    {
        HP -= damage;
        anim.SetBool("isDamaging", true);
        myRb.AddForce(new Vector2(50, 100));
    }
    public override void FinishDamaged()
    {
        anim.SetBool("isDamaging", false);
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheckFlip.position, new Vector2(groundCheckFlip.transform.position.x, groundCheckFlip.transform.position.y - checkGroundDistance));
        Gizmos.DrawLine(wallCheckFlip.position, new Vector2(wallCheckFlip.transform.position.x + checkWallDistance,wallCheckFlip.transform.position.y));
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
