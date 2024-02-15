using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private enum State
    {
        Combat,
        Knockback,
        Dead
    }

    private State currentState;

    [SerializeField]
    private float
        rangeSkill2,
        movementSpeed,
        maxHealth,
        knockbackDuration,
        posPlayerCheckDistance,
        timeDashing;

    [SerializeField]
    private LayerMask 
        whatIsGround,
        whatIsPlayer;

    [SerializeField]
    private Vector2 knockbackSpeed;

    [SerializeField]
    private Transform
        LeftTop,
        LeftBot,
        RightTop,
        RightBot;

    private float
        currentHealth,
        knockbackStartTime,
        dashTimeLeft,

        TimeSkill1CoolDown = 3f,
        attackTimeLeftSkill1 = 0,
        TimeSkill1Fire = 0.5f,

        TimeSkill2CoolDown = 3f,
        attackTimeLeftSkill2 = 0,
        TimeSkill2Fire = 0.75f;
        
    private int 
        facingDirection,
        countSkill1 = 0,
        countSkill2 = 0;

    private Vector2 posStart;

    private bool
        playerDetected,
        canSkill1,
        canSkill2,
        canSkill4,
        canTele,
        canDash,
        canFlip;

    private Rigidbody2D myRb;
    private GameObject player;
    public GameObject enemy1;
    public GameObject enemy2;

    private void Start()
    {
        myRb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        facingDirection = 1;
        currentState = State.Combat;

        canTele = true;
        dashTimeLeft = timeDashing;
        canFlip = true;

        posStart = transform.position;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Combat:
                UpdateCombatState();
                break;
            case State.Knockback:
                UpdateKnockbackState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;
        }
    }

    //--COMBAT STATE-----------------------------------------------------

    private void EnterCombatState()
    {
    }

    private void UpdateCombatState()
    {
        CheckPositionPlayer();
        CheckCombat();
        //Skill1();
        //Skill2();
        //Skill3();
        Skill4();
    }

    private void ExitCombatState()
    {

    }

    //--KNOCKBACK STATE-------------------------------------------------
    private void EnterKnockbackState()
    {
        
    }

    private void UpdateKnockbackState()
    {

    }

    private void ExitKnockbackState() 
    {
        //aliveAnim.SetBool("Knockback", false);
    }
    
    //--DEAD STATE------------------------------------------------------
    private void EnterDeadState()
    {
        Destroy(gameObject);
    }

    private void UpdateDeadState()
    {

    }
    
    private void ExitDeadState()
    {

    }

    //--OTHER FUNCTIONS---------------------------------------------------

    private void Flip()
    {
        if (canFlip)
        {
            facingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void CheckPositionPlayer()
    {
        playerDetected = Physics2D.OverlapCircle(this.transform.position, posPlayerCheckDistance, whatIsPlayer);
        if (playerDetected)//Phát hiện player và tự động Flip()
        {
            Collider2D detectObj = Physics2D.OverlapCircle(this.transform.position, posPlayerCheckDistance, whatIsPlayer);
            player = detectObj.gameObject;
            if(player != null)
            {
                float temp = this.transform.position.x - player.transform.position.x;
                if(temp > 0.0f && facingDirection == 1) 
                {
                    Flip();
                }
                else if(temp <= 0.0f && facingDirection == -1)
                {
                    Flip();
                }
            }
        }
        else
        {
            player = null;
        }
    }
    private void CheckCombat()
    {
        canSkill1 = Physics2D.OverlapCircle(this.transform.position, posPlayerCheckDistance, whatIsPlayer);
        canSkill2 = Physics2D.OverlapCircle(this.transform.position, rangeSkill2, whatIsPlayer);
    }

    private void SwitchState(State state)
    {
        switch (currentState)
        {
            case State.Combat:
                ExitCombatState();
                break;
            case State.Knockback:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
        }

        switch (state)
        {
            case State.Combat:
                EnterCombatState();
                break;
            case State.Knockback:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }
    }

    private void Skill1()//Tha cau lua
    {

        if(player != null && canSkill2)
        {
            TimeSkill1Fire += Time.deltaTime;

            if(TimeSkill1Fire >= 0.5f)
            {
                GameObject b1 = BulletManager.Instance.TakeFireBallBoss();
                b1.transform.position = new Vector2(player.transform.position.x - 3, 12);

                GameObject b2 = BulletManager.Instance.TakeFireBallBoss();
                b2.transform.position = new Vector2(player.transform.position.x - 1, 12);

                GameObject b3 = BulletManager.Instance.TakeFireBallBoss();
                b3.transform.position = new Vector2(player.transform.position.x + 1, 12);

                GameObject b4 = BulletManager.Instance.TakeFireBallBoss();
                b4.transform.position = new Vector2(player.transform.position.x + 3, 12);

                TimeSkill1Fire = 0;
                countSkill1++;
            }
                
            if(countSkill1 == 4)
            {
                attackTimeLeftSkill1 = TimeSkill1CoolDown;
                TimeSkill1Fire = 0.5f;
                countSkill1 = 0;
            }
        }
    }
    private void Skill2()// Trieu hoi bomb
    {
        if(player!=null  && canSkill2)
        {
            TimeSkill2Fire += Time.deltaTime;

            if(TimeSkill2Fire >= 0.75f)
            {
                GameObject b = BulletManager.Instance.TakeBombBoss();
                b.transform.position = player.transform.position;

                countSkill2++;
                TimeSkill2Fire = 0;
            }

            if(countSkill2 == 2)
            {
                attackTimeLeftSkill2 = TimeSkill2CoolDown;
                countSkill2 = 0;
            }
        }
    }
    private void Skill3()// Trieu hoi Enemy
    {
        GameObject e1 = Instantiate(enemy1);
        e1.transform.position = new Vector2(transform.position.x - 1, transform.position.y - 1);
        GameObject e2 = Instantiate(enemy2);
        e2.transform.position = new Vector2(transform.position.x + 1, transform.position.y - 1);
    }
    private void Skill4()// huc tong
    {
        if (canTele)
        {
            this.transform.position = LeftBot.transform.position;
            canTele = false;
            canDash = true;
        }
        if (canDash)
        {
            if(dashTimeLeft > 0)
            {
                canFlip = false;
                myRb.velocity = new Vector2((RightBot.position.x - LeftBot.position.x)/timeDashing,0);
                dashTimeLeft -= Time.deltaTime;
            }
            else
            {
                myRb.velocity = Vector2.zero;
                canFlip = true;
                canDash = false;
            }
        }
        if(!canTele && !canDash)
        {
            this.transform.position = posStart;
            //canTele = true;
            //dashTimeLeft = timeDashing;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, posPlayerCheckDistance);
        Gizmos.DrawWireSphere(this.transform.position, rangeSkill2);

        Gizmos.DrawWireSphere(LeftBot.position, 1);
        Gizmos.DrawWireSphere(LeftTop.position, 1);
        Gizmos.DrawWireSphere(RightTop.position, 1);
        Gizmos.DrawWireSphere(RightBot.position, 1);

    }
}
