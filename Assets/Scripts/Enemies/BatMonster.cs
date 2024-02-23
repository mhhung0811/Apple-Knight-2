using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BatMonster : BaseEnemy
{
    [SerializeField]
    private EnemyData enemyData;

    private bool playerDetected, canAttack;

    private Animator anim;
    private Rigidbody2D myRb;
    private GameObject player;

    private float HP, attackCoolDown;

    public LayerMask whatIsPlayer;

    void Start()
    {
        InitializedEnemy();
    }
    public override void InitializedEnemy()
    {
        HP = enemyData.maxHP;
        anim = GetComponent<Animator>();
        myRb = GetComponent<Rigidbody2D>();
        canMove = true;
        facingDirection = 1f;
        attackCoolDown = 1f;
    }
    void Update()
    {
        if (InGameManager.Instance.PauseGame())
        {
            return;
        }

        DetectPlayer();
        DealDamage();
    }
    private void FixedUpdate()
    {
        if (InGameManager.Instance.PauseGame())
        {
            return;
        }
        Move();
    }

    public override void DealDamage()
    {
        CheckHitBoxAttack();
    }
    public override void Move()
    {
        if(player != null)
        {
            Vector2 facing = player.transform.position - this.transform.position;
            myRb.velocity = facing.normalized * enemyData.speed;
        }
        else
        {
            myRb.velocity = Vector3.zero;
        }
    }
    public override void DetectPlayer()
    {
        playerDetected = Physics2D.OverlapCircle(transform.position, enemyData.detectionRange, whatIsPlayer);
        if (playerDetected)
        {
            Collider2D detectObj = Physics2D.OverlapCircle(transform.position, enemyData.detectionRange, whatIsPlayer);
            player = detectObj.gameObject;
            if (player != null)
            {
                float temp = this.transform.position.x - player.transform.position.x;
                if (temp > 0.0f && facingDirection == 1)
                {
                    Flip();
                }
                else if (temp <= 0.0f && facingDirection == -1)
                {
                    Flip();
                }
            }
        }
    }
    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
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
            InGameManager.Instance.IncreaseExp(enemyData.exp);
            Destroy(this.gameObject);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, enemyData.detectionRange);
    }
    private void CheckHitBoxAttack()
    {
        bool detectObj = Physics2D.OverlapCircle(transform.position, 0.5f, whatIsPlayer);
        if(detectObj)
        {
            StartCoroutine(AttackTime());
        }
    }
    private IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(0.2f);
        if(player!=null)
        {
            player.GetComponent<PlayerCombatController>().TakeDamage(enemyData.damage, this.gameObject, 0f);
            Destroy(this.gameObject);
        }
    }
}
