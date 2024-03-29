using System.Collections;
using UnityEngine;

public class Enemy2 : BaseEnemy
{
    [SerializeField]
    private EnemyData enemyData;
    [SerializeField]
    private MageHoodedAnimation animCtrl;

    private Rigidbody2D myRb;
    private bool isDetectPlayerLeft;
    private bool isDetectPleyerRight;

    public float attackCoolDown;
    public float attackTimeLeft;
    private float attackRadius;
    private float HP;
    public bool isAttack;
    public bool isFire;
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

        if (isFire && !isAttack)
        {
            if (attackTimeLeft > 0)
            {
                attackTimeLeft -= Time.deltaTime;
                canMove = false;
            }
            else
            {
                animCtrl.StartCast(facingDirection);
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
        GameObject b = BulletManager.Instance.TakeFireBall();
        b.transform.position = this.transform.position;
        FireBall fire = b.GetComponent<FireBall>();
        fire.SetUp(facingDirection);
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
        isDetectPlayerLeft = Physics2D.Raycast(detectPlayer.position + transform.right.normalized*-1, transform.right * -1, enemyData.detectionRange -1, whatIsPlayer);
        isDetectPleyerRight = Physics2D.Raycast(detectPlayer.position, transform.right, enemyData.detectionRange, whatIsPlayer);
        isFacingLeft = Physics2D.Raycast(new Vector2(detectPlayer.position.x - enemyData.detectionRange, detectPlayer.position.y), transform.right, enemyData.detectionRange, whatIsPlayer);

        if (isDetectPleyerRight)
        {
            isFire = true;
        }
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
        Debug.Log("bugging");
        if (HP <= 0)
        {
            SaveDataManager.Instance.SaveEnemyInGameData(id);
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
        Gizmos.DrawLine(detectPlayer.position - new Vector3(enemyData.detectionRange, 0, 0), detectPlayer.position + new Vector3(enemyData.detectionRange, 0, 0));
    }
}
