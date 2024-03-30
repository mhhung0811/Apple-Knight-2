using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : BaseEnemy
{
    [SerializeField]
    private EnemyData enemyData;
    [SerializeField]
    private BomberAnimation animCtrl;

    private Rigidbody2D myRb;
    private bool isDetectPlayerLeft;
    private bool isDetectPleyerRight;
    private bool isDetectPlayerCircle;
    private GameObject player;

    private float attackCoolDown;
    private float attackTimeLeft;
    private float attackRadius;
    private float HP;
    private bool isAttack;
    private bool isFire;
    private bool isStun;

    public Transform detectPlayer;
    public Transform attackHitBoxPos;
    public LayerMask whatIsPlayer;
    
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
        attackCoolDown = 1.5f;
        attackTimeLeft = 1f;
        attackRadius = 1.5f;
        facingDirection = 1f;
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

        if (!isAttack)
        {
            if (attackTimeLeft > 0)
            {
                attackTimeLeft -= Time.deltaTime;
                canMove = false;
            }
            else
            {
                if(player != null)
                {
                    animCtrl.StartCast(facingDirection);
                }
            }
        }
    }
    public void Attacking()
    {
        GameObject slash = EffectManager.Instance.Take(EFFECTTYPE.Slash);
        slash.transform.position = attackHitBoxPos.position;
        Slash s = slash.GetComponent<Slash>();
        s.StartSlash();
        s.CanFlip(facingDirection);

        CheckAttackHitBox();
        attackTimeLeft = attackCoolDown;
        isAttack = false;
    }
    public void Casting()
    {
        GameObject b = BulletManager.Instance.TakeBomb();
        b.transform.position = this.transform.position;
        Bomb bom = b.GetComponent<Bomb>();
        float H, L;
        H = player.transform.position.y - this.transform.position.y;
        L = player.transform.position.x - this.transform.position.x;
        if (L > 0 && facingDirection == -1)
        {
            Flip();
            facingDirection = 1;
        }
        if (L < 0 && facingDirection == 1)
        {
            Flip();
            facingDirection = -1;
        }
        L = Mathf.Abs(L);
        bom.SetUp(H, L, facingDirection);
        isFire = false;
        attackTimeLeft = attackCoolDown;
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
    }
    public void Flip()
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
    public override void Move()
    {
        myRb.velocity = new Vector2(enemyData.speed * 0, myRb.velocity.y);
    }
    public override void DetectPlayer()
    {
        isDetectPlayerLeft = Physics2D.Raycast(detectPlayer.position + transform.right.normalized * -1, transform.right * -1, enemyData.detectionRange - 1, whatIsPlayer);
        isDetectPleyerRight = Physics2D.Raycast(detectPlayer.position, transform.right, enemyData.detectionRange, whatIsPlayer);
        isFacingLeft = Physics2D.Raycast(new Vector2(detectPlayer.position.x - enemyData.detectionRange, detectPlayer.position.y), transform.right, enemyData.detectionRange, whatIsPlayer);
        isDetectPlayerCircle = Physics2D.OverlapCircle(detectPlayer.position, enemyData.detectionRange, whatIsPlayer);
       
        if (isDetectPlayerCircle)
        {
            Collider2D detectObj = Physics2D.OverlapCircle(detectPlayer.position,enemyData.detectionRange, whatIsPlayer);
            player = detectObj.gameObject;
        }
        else
        {
            player = null;
        }
    }
    public override void IsDamaged(float damage)
    {
        HP -= damage;
        animCtrl.StartDamaged();
        myRb.AddForce(new Vector2(50, 100));
        StartCoroutine(CanStun());
        if (HP <= 0)
        {
            SaveDataManager.Instance.SaveEnemyInGameData(id);
            InGameManager.Instance.IncreaseExp(enemyData.exp);
            Destroy(this.gameObject);
        }
    }
    public override void FinishDamaged()
    {
        //if (HP <= 0)
        //{
        //    SaveDataManager.Instance.SaveEnemyInGameData(id);
        //    InGameManager.Instance.IncreaseExp(enemyData.exp);
        //    Destroy(this.gameObject);
        //}
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
        Gizmos.DrawLine(detectPlayer.position - new Vector3(enemyData.detectionRange, 0, 0), detectPlayer.position + new Vector3(enemyData.detectionRange, 0, 0));
        Gizmos.DrawWireSphere(detectPlayer.position,enemyData.detectionRange);
    }
}
