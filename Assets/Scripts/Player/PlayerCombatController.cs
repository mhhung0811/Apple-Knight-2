using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField]
    private bool combatEnable;
    [SerializeField]
    private bool hitBoxAppearance;
    [SerializeField]
    private float inputTimer, combatTime, attackRadius;
    [SerializeField]
    private Transform attackHitBoxPos;
    [SerializeField]
    private LayerMask WhatIsDamageable;

    [SerializeField]
    private PlayerData playerData;

    private bool gotInput, isCombat, isAttacking1, isAttacking2;

    private int countAttack;

    private float lastInputTime;

    public float PercentDamage;

    [SerializeField]
    //private Animator anim;
    private PlayerAnimation animCtrl;

    [SerializeField]
    private PlayerEffect animeffect;

    private Rigidbody2D myRb;

    private float HP;
    private float MaxHP;
    private float HPEachSecond;

    private void Start()
    {
        myRb = GetComponent<Rigidbody2D>();

        lastInputTime = Mathf.NegativeInfinity;
        countAttack = 0;
        isCombat = false;
        
        MaxHP = HP = playerData.maxHP;
        HPEachSecond = 0;
        PercentDamage = 1;
    }
    private void Update()
    {
        if (InGameManager.Instance.PauseGame())
        {
            return;
        }
        CheckCombarInput();
        CheckAttack();
    }
    public void HoiHP(float hPEachSecond)
    {
        HPEachSecond = hPEachSecond;
        InvokeRepeating("IncreaseHP", 1f, 1f);
    }
    private void IncreaseHP()
    {
        HP+= HPEachSecond;

        if(HP > MaxHP)
        {
            HP = MaxHP;
        }
        UIManager.Instance.SetHPUi(HP, MaxHP);
    }
    public void TangHP(float maxHp)
    {
        MaxHP = maxHp;
        HP += 50;
        if(HP > MaxHP)
        {
            HP = MaxHP;
        }
        UIManager.Instance.SetHPUi(HP, MaxHP);
    }
    private void CheckCombarInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AudioManager.Instance.PlaySound("Attack");
            if (combatEnable)
            {
                // Atempt combat
                gotInput = true;
                //lastInputTime = Time.time;
            }
        }
        // Last input time over combat time -> reset combat
        if (Time.time > lastInputTime + combatTime)
        {
            countAttack = 0;
            animCtrl.StartAttack(-1);
            //animeffect.StartAttack1();
            //anim.SetInteger("countAttack", countAttack);
            //isCombat = false;
            //anim.SetBool("isCombat", isCombat);
            //animCtrl.ToIdle();
        }
    }
    private void CheckAttack()
    {
        if (gotInput)
        {
            // Apply input
            gotInput = false;
            lastInputTime = Time.time;
            animCtrl.StartAttack(countAttack);
            CheckAttackHitBox();
            if (countAttack == 0)
            {
                countAttack++;
            }
            else
            {
                countAttack = 0;
            }
            //// Start attack
            //if (!isCombat)
            //{
            //    // Apply input
            //    gotInput = false;
            //    animCtrl.setCountAttack(countAttack);

            //    // Start combat
            //    isCombat = true;

            //    // Perform attack 1
            //    //animCtrl.ToIdle();
            //    animCtrl.StartAttack1();

            //    countAttack = 1;
            //    CheckAttackHitBox();
            //}
            //// is combating
            //else
            //{
            //    // Apply input
            //    gotInput = false;
            //    animCtrl.setCountAttack(countAttack);

            //    if (countAttack == 0)
            //    {
            //        // Perform attack 1
            //        //animCtrl.ToIdle();
            //        animCtrl.StartAttack1();

            //        countAttack = 1;
            //        CheckAttackHitBox();
            //    }

            //    if (countAttack == 1)
            //    {
            //        // Perform attack 2
            //        //animCtrl.ToIdle();
            //        animCtrl.StartAttack2();

            //        countAttack = 0;
            //        CheckAttackHitBox();
            //    }
            //}
        }
        if (Time.time < lastInputTime + inputTimer)
        {
            // Wait for new input
            gotInput = false;
        }
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackHitBoxPos.position, attackRadius, WhatIsDamageable);
        if (detectedObjects == null) return;
        foreach (Collider2D coll in detectedObjects)
        {
            // Enemy attack
            if(coll.gameObject.CompareTag("Enemy") || coll.gameObject.CompareTag("Boss"))
            {
                coll.transform.SendMessage("IsDamaged", playerData.damage*PercentDamage);
            }
            // Trigger interactable object
            if (coll.gameObject.CompareTag("Interactable Object"))
            {
                coll.gameObject.GetComponent<IInteractable>().InteractOn();
            }
            // Instantiate hit particle
        }
    }
    //private void FinishAttack1()
    //{
    //    isAttacking1 = false;
    //    anim.SetBool("isAttacking1", isAttacking1);
    //    countAttack = 1;
    //    anim.SetInteger("countAttack", countAttack);
    //}
    //private void FinishAttack2()
    //{
    //    isAttacking2 = false;
    //    anim.SetBool("isAttacking2", isAttacking2);
    //    // Last attack
    //    countAttack = 0;
    //    anim.SetInteger("countAttack", countAttack);

    //}

    public void TakeDamage(float damaged, GameObject enemy, float knockback)
    {
        if (damaged > 0)
        {
            HP -= damaged;
            animCtrl.StartDamaged();
        }

        float temp = transform.position.y - enemy.transform.position.y;
        if (temp >= 0)
        {
            myRb.velocity = new Vector2(myRb.velocity.x, knockback);
        }
        else
        {
            myRb.velocity = new Vector2(myRb.velocity.x, -knockback);
        }
        UIManager.Instance.SetHPUi(HP,MaxHP);
        Die();
    }

    public void Die()
    {
        if(HP <= 0)
        {
            Destroy(this.gameObject);
            InGameManager.Instance.GameOver();
        }
    }
    private void OnDrawGizmos()
    {
        if(hitBoxAppearance)
        {
            Gizmos.DrawSphere(attackHitBoxPos.position, attackRadius);
        }
    }
    //UI button
    public void ButtonAttack()
    {
        AudioManager.Instance.PlaySound("Attack");
        if (combatEnable)
        {
            // Atempt combat
            gotInput = true;
            //lastInputTime = Time.time;
        }
    }
}
