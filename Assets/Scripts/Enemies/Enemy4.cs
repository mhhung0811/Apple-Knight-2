using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy4 : BaseEnemy
{
    //private Animator anim;

    [SerializeField]
    private EnemyData enemyData;
    [SerializeField]
    private SkeletonAnimation animCtrl;
    
    private Rigidbody2D myRb;
    private bool isDetectPlayerLeft;
    private bool isDetectPleyerRight;

    public float attackCoolDown;
    public float attackTimeLeft;
    private float attackRadius;
    private float HP;
    private float checkGroundDistance = 0.5f;
    private float checkWallDistance = 0.5f;
    private float groundCheckRadius = 0.4f;

    public bool isAttack;
    private bool isGroundedFlip;
    private bool isWallFlip;
    private bool isGround;
    private bool isStun;

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
        HP = enemyData.maxHP;
        
        myRb = GetComponent<Rigidbody2D>();
        isDetectPlayerLeft = isDetectPleyerRight = false;
        canMove = true;
        attackCoolDown = 1f;
        attackTimeLeft = 1f;
        attackRadius = 1.5f;
        facingDirection = 1;
        isStun = true;
    }
    void Update()
    {
        if (InGameManager.Instance.PauseGame())
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
        if (InGameManager.Instance.PauseGame())
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
                animCtrl.StartAttack(facingDirection);
            }
        }
        else
        {
            attackTimeLeft = attackCoolDown;
            canMove = true;
        }
    }
    public void Attaking()
    {
        GameObject slash = EffectManager.Instance.Take(1);
        slash.transform.position = attackHitBoxPos.position;
        Slash s = slash.GetComponent<Slash>();
        s.StartSlash();
        s.CanFlip(facingDirection);

        CheckAttackHitBox();
        attackTimeLeft = attackCoolDown;
        isAttack = false;
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
            animCtrl.StartWalk();
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
            animCtrl.StartWalk();
            myRb.velocity = new Vector2((enemyData.speed+2) * facingDirection, myRb.velocity.y);
        }
        else
        {
            animCtrl.EndWalk();
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
        animCtrl.StartDamaged();
        myRb.AddForce(new Vector2(50, 100));
        StartCoroutine(CanStun());
    }
    public override void FinishDamaged()
    {
        if (HP <= 0)
        {
            InGameManager.Instance.IncreaseExp(enemyData.exp);
            Destroy(this.gameObject);
        }
    }
    private IEnumerator CanStun()
    {
        if (isStun)
        {
            attackTimeLeft = attackCoolDown;
            isStun = false;
            yield return new WaitForSeconds(2f);
            isStun = true;
        }
        else
        {
            yield return new WaitForSeconds(0f);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheckFlip.position, new Vector2(groundCheckFlip.transform.position.x, groundCheckFlip.transform.position.y - checkGroundDistance));
        Gizmos.DrawLine(wallCheckFlip.position, new Vector2(wallCheckFlip.transform.position.x + checkWallDistance,wallCheckFlip.transform.position.y));
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
