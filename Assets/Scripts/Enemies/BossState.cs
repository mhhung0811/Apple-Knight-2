using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class BossState : MonoBehaviour
{
    private enum State
    {
        Skill1,
        Skill2,
        Skill3,
        Skill4,
        Wait
    }

    private State currentState;

    [SerializeField]
    private float playerDetectionRange, checkDodgeRange, HP;

    private float numberOfFireballs, numberOfBomb,
        dashTimeLeft, timeDashing, 
        dodgeCoolDown, timeDodgeDown, dodgeDashTimeLeft,timeDodgeDashing;

    private bool playerDetected, canFlip, canDash, canCombat, canSpawnEnemy, canDamageSkill4, canDodge, canDodgeDash, isPosInit;

    private int facingDirection;

    [SerializeField]
    private LayerMask whatIsGround, whatIsPlayer;

    private Vector2 posInit;
    private Rigidbody2D myRb;
    private GameObject player;
    private Animator anim;
    public GameObject enemy1;
    public GameObject enemy2;

    [SerializeField]
    private Transform LeftTop, LeftBot, RightTop, RightBot;

    void Start()
    {
        anim = GetComponent<Animator>();
        myRb = GetComponent<Rigidbody2D>();
        posInit = this.transform.position;

        HP = 200;
        facingDirection = 1;
        playerDetectionRange = 20;
        numberOfFireballs = 4;
        numberOfBomb = 2;
        checkDodgeRange = 2.5f;
        timeDashing = dashTimeLeft = 1f;

        timeDodgeDown = 0; dodgeCoolDown = 15f;
        dodgeDashTimeLeft = timeDodgeDashing = 0.1f;

        canFlip = true;
        canSpawnEnemy = false;
        canDash = false;
        canCombat = false;
        canDamageSkill4 = false;
        isPosInit = true;

        SwitchState(State.Wait);
    }

    void Update()
    {
        if (GameManager.Instance.PauseGame())
        {
            return;
        }

        CheckPosPleyerAndAutoFlip();
        CheckDodgeAttack();

        switch (currentState)
        {
            case State.Skill1:
                UpdateSkill1State();
                break;
            case State.Skill2: 
                UpdateSkill2State(); 
                break;
            case State.Skill3:
                UpdateSkill3State();
                break;
            case State.Skill4:
                UpdateSkill4State();
                break;
            case State.Wait: 
                UpdateWaitState(); 
                break;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            SwitchState(State.Skill1);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            SwitchState(State.Skill2);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchState(State.Skill3);
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            SwitchState(State.Skill4);
        }
    }
    // Skill1 State (Xác định vị trí Pleyer, sau đó thả cầu lửa từ trên trời xuống)
    private void EnterSkill1State()
    {
        StartCoroutine(SpawnFireballs());
    }
    private void UpdateSkill1State()
    {

    }
    private void ExitSkill1State()
    {
    }
    // Skill2 State (Tạo ra một quả boom tại vị trí của Player và phát nổ sau đó (vd: 0.75s))
    private void EnterSkill2State()
    {
        StartCoroutine(SpawnBomb());
    }
    private void UpdateSkill2State()
    {

    }
    private void ExitSkill2State()
    {
    }
    // Skill3 State (Tạo thêm 2 enemy)
    private void EnterSkill3State()
    {
        canSpawnEnemy = true;
    }
    private void UpdateSkill3State()
    {
        if (canSpawnEnemy)
        {
            GameObject e1 = Instantiate(enemy1);
            e1.transform.position = new Vector2(transform.position.x - 1, transform.position.y - 1);
            GameObject e2 = Instantiate(enemy2);
            e2.transform.position = new Vector2(transform.position.x + 1, transform.position.y - 1);
            SwitchState(State.Wait);
            canSpawnEnemy = false;
        }
    }
    private void ExitSkill3State()
    {
        canSpawnEnemy = false;
    }
    // Skill4 State (Lướt xuống 2 góc tường, sau đó bay đâm ngang qua)
    private void EnterSkill4State()
    {
        //Teleport
        StartCoroutine(Teleport(LeftBot.transform.position, true));
        canFlip = false;
    }
    private void UpdateSkill4State()
    {
        //Dash
        if (canDash)
        {
            if (dashTimeLeft > 0)
            {
                myRb.velocity = new Vector2((RightBot.position.x - LeftBot.position.x) / timeDashing, 0);
                dashTimeLeft -= Time.deltaTime;
                CheckHitBoxAttackSkill4();
            }
            else
            {
                myRb.velocity = Vector2.zero;
                canFlip = true;
                canDash = false;
                StartCoroutine(Teleport(posInit, false));
            }
        }
    }
    private void ExitSkill4State()
    {
        dashTimeLeft = timeDashing;
    }
    // Wait State (Chờ giữa các trạng thái)
    private void EnterWaitState()
    {
        StartCoroutine(Wait(2f));
    }
    private void UpdateWaitState()
    {
        if (canCombat)
        {
            int index = UnityEngine.Random.Range(1, 5);
            switch (index)
            {
                case 1:
                    SwitchState(State.Skill1);
                    canCombat = false;
                    break;
                case 2:
                    SwitchState(State.Skill2);
                    canCombat = false;
                    break;
                case 3:
                    SwitchState(State.Skill3);
                    canCombat = false;
                    break;
                case 4:
                    SwitchState(State.Skill4);
                    canCombat = false;
                    break;
            }
        }
    }
    private void ExitWaitState()
    {
        canCombat = false;
    }

    // Other function
    private void Flip()
    {
        if (canFlip)
        {
            facingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void CheckPosPleyerAndAutoFlip()
    {
        playerDetected = Physics2D.OverlapCircle(transform.position, playerDetectionRange, whatIsPlayer);
        if (playerDetected)
        {
            Collider2D detectObj = Physics2D.OverlapCircle(transform.position, playerDetectionRange, whatIsPlayer);
            player = detectObj.gameObject;
            if(player != null)
            {
                float comparePosX = player.transform.position.x - this.transform.position.x;
                if(comparePosX < 0 && facingDirection == 1)
                {
                    Flip();
                }
                else if(comparePosX >= 0 && facingDirection == -1)
                {
                    Flip();
                }
            }
        }
    }
    
    private void SwitchState(State state)
    {
        switch(currentState)
        {
            case State.Skill1:
                ExitSkill1State();
                break;
            case State.Skill2:
                ExitSkill2State();
                break;
            case State.Skill3:
                ExitSkill3State();
                break;
            case State.Skill4:
                ExitSkill4State();
                break;
            case State.Wait:
                ExitWaitState();
                break;
        }

        switch (state)
        {
            case State.Skill1:
                EnterSkill1State();
                break;
            case State.Skill2:
                EnterSkill2State();
                break;
            case State.Skill3:
                EnterSkill3State();
                break;
            case State.Skill4:
                EnterSkill4State();
                break;
            case State.Wait:
                EnterWaitState();
                break;
        }

        currentState = state;
    }

    private void CheckHitBoxAttackSkill4()
    {
        if(canDamageSkill4)
        {
            Collider2D detectPlayer = Physics2D.OverlapCircle(this.transform.position + transform.right.normalized, 0.75f, whatIsPlayer);
            if (detectPlayer)
            {
                GameObject pl = detectPlayer.gameObject;
                pl.GetComponent<PlayerCombatController>().TakeDamage(10, this.gameObject, 0);
                canDamageSkill4 = false;
            }
        }
    }

    private void CheckDodgeAttack()
    {
        canDodge = Physics2D.OverlapCircle(transform.position, checkDodgeRange, whatIsPlayer);
        timeDodgeDown -= Time.deltaTime;
        if (canDodge)
        {
            if(timeDodgeDown <= 0 && currentState != State.Skill4)
            {
                if(dodgeDashTimeLeft > 0)
                {
                    canDodgeDash = true;
                    isPosInit = false;
                }
            }
        }
        if(canDodgeDash == true && dodgeDashTimeLeft > 0)
        {
            myRb.velocity = new Vector2(-facingDirection * 8 / timeDodgeDashing, 0);
            dodgeDashTimeLeft -= Time.deltaTime;
        }
        if (timeDodgeDown <=0 && dodgeDashTimeLeft <= 0)
        {
            myRb.velocity = Vector2.zero;
            timeDodgeDown = dodgeCoolDown;
            dodgeDashTimeLeft = timeDodgeDashing;
            canDodgeDash = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, playerDetectionRange);

        Gizmos.DrawWireSphere(LeftBot.position, 1);
        Gizmos.DrawWireSphere(LeftTop.position, 1);
        Gizmos.DrawWireSphere(RightTop.position, 1);
        Gizmos.DrawWireSphere(RightBot.position, 1);

        Gizmos.DrawWireSphere(this.transform.position + transform.right.normalized, 0.75f);

        Gizmos.DrawWireSphere(transform.position, 2.5f);
    }
    
    private IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canCombat = true;
    }
    private IEnumerator SpawnFireballs()
    {
        for(int i = 0; i < numberOfFireballs; i++)
        {
            if(player == null)
            {
                break;
            }

            GameObject b1 = BulletManager.Instance.TakeFireBallBoss();
            b1.transform.position = new Vector2(player.transform.position.x - 2.25f, 12);

            GameObject b2 = BulletManager.Instance.TakeFireBallBoss();
            b2.transform.position = new Vector2(player.transform.position.x - 0.75f, 12);

            GameObject b3 = BulletManager.Instance.TakeFireBallBoss();
            b3.transform.position = new Vector2(player.transform.position.x + 0.75f, 12);

            GameObject b4 = BulletManager.Instance.TakeFireBallBoss();
            b4.transform.position = new Vector2(player.transform.position.x + 2.25f, 12);

            yield return new WaitForSeconds(0.5f);
        }

        SwitchState(State.Wait);
    }
    private IEnumerator SpawnBomb()
    {
        for(int i = 0; i < numberOfBomb; i++)
        {
            if (player == null)
            {
                break;
            }
            GameObject b = BulletManager.Instance.TakeBombBoss();
            b.transform.position = player.transform.position;
            yield return new WaitForSeconds(0.75f);
        }
        SwitchState(State.Wait);
    }
    private IEnumerator Teleport(Vector2 posTele, bool CanDash)
    {
        if (!isPosInit)
        {
            yield return new WaitForSeconds(1f);
            anim.SetBool("IsPreTele", true);

            // Tele tới đích
            yield return new WaitForSeconds(0.5f);
            this.transform.position = posInit;
            anim.SetBool("IsTele", true);

            // Đã tele tới đích
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("IsTele", false);
            anim.SetBool("IsPreTele", false);

            SwitchState(State.Wait);
            isPosInit = true;
        }
        else
        {
            // Chờ 1 chút rồi bắt đầu tele;
            yield return new WaitForSeconds(1f);
            anim.SetBool("IsPreTele", true);

            // Tele tới đích
            yield return new WaitForSeconds(0.5f);
            this.transform.position = posTele;
            anim.SetBool("IsTele", true);

            // Đã tele tới đích
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("IsTele", false);
            anim.SetBool("IsPreTele", false);
            canDash = CanDash;
            if(canDash == false)
            {
                SwitchState(State.Wait);
            }
            else
            {
                canDamageSkill4 = true;
            }
        }
    }

    public void IsDamaged(float damage)
    {
        HP -= damage;
        anim.SetBool("isDamaging", true);
    }
    public void FinishDamaged()
    {
        anim.SetBool("isDamaging", false);
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
