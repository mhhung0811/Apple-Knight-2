using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemy1: BaseEnemy
{
    protected Animator anim;
    protected Rigidbody2D myRb;
    protected bool isDetectPlayerLeft;
    protected bool isDetectPleyerRight;
    protected bool isFacingRight;

    private float attackCoolDown;
    private float attackTime;

    public Transform detectPlayer; 
    public LayerMask whatIsPlayer;

    void Start()
    {
        InitializedEnemy();
    }
    public override void InitializedEnemy()
    {
        enemyData = new EnemyData();
        enemyData.maxHP = 100;
        enemyData.damage = 10;
        enemyData.HP = enemyData.maxHP;
        enemyData.speed = 3;
        enemyData.detectionRange = 10;
        anim = GetComponent<Animator>();
        myRb = GetComponent<Rigidbody2D>();
        isDetectPlayerLeft = isDetectPleyerRight = false;
        canMove = true;
        attackCoolDown = 1f;
        attackTime = 1f;
    }
    void Update()
    {
        DetectPlayer();
        CheckMoveDirection();
        DealDamage();
    }
    private void FixedUpdate()
    {
        Move();
    }

    public override void DealDamage()
    {
        isDetectInHitBox = Physics2D.Raycast(this.transform.position, transform.right, 1.5f, whatIsPlayer);
        if (isDetectInHitBox)
        {
            if(attackTime > 0)
            {
                attackTime -= Time.deltaTime;
                canMove = false;
            }
            else
            {
                anim.SetBool("isAttack", true);
            }
        }
        else
        {
            attackTime = attackCoolDown;
            canMove = true; 
        }
    }
    public void FinishAttack1()
    {
        anim.SetBool("isAttack", false);
        canMove = true;
        attackTime = attackCoolDown;
    }
    public void CheckMoveDirection()
    {
        if(isDetectPlayerLeft || isDetectPleyerRight) 
        {
            Flip();
        }
    }
    public void Flip()
    {
        if (isDetectPlayerLeft)
        {
            transform.Rotate(0, 180, 0);
            if (isFacingRight)
            {
                facingDirection = -1;
            }
            else
            {
                facingDirection = 1;
            }
        }
    }
    public override void Move()
    {
        if (isDetectPleyerRight && canMove)
        {
            myRb.velocity = new Vector2(enemyData.speed*facingDirection,myRb.velocity.y);
        }
        else
        {
            myRb.velocity = new Vector2(enemyData.speed * 0, myRb.velocity.y);
        }
    }
    public override void DetectPlayer()
    {
        isDetectPlayerLeft = Physics2D.Raycast(detectPlayer.position, transform.right * -1, enemyData.detectionRange, whatIsPlayer);
        isDetectPleyerRight = Physics2D.Raycast(detectPlayer.position, transform.right, enemyData.detectionRange, whatIsPlayer);
        isFacingRight = Physics2D.Raycast(new Vector2(detectPlayer.position.x-enemyData.detectionRange,detectPlayer.position.y), transform.right, enemyData.detectionRange, whatIsPlayer);
    }
    public override void IsDamaged(float damage)
    {
        enemyData.HP -= damage;
        anim.SetBool("isDamaging", true);
        myRb.AddForce(new Vector2(50, 100));
    }
    public override void FinishDamaged()
    {
        anim.SetBool("isDamaging", false);
        if (enemyData.HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
