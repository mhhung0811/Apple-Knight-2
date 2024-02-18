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

    [SerializeField]
    //private Animator anim;
    private PlayerAnimation animCtrl;

    private Rigidbody2D myRb;

    private float HP;

    private void Start()
    {
        myRb = GetComponent<Rigidbody2D>();

        lastInputTime = Mathf.NegativeInfinity;
        countAttack = 0;
        isCombat = false;
        
        HP = playerData.maxHP;
    }
    private void Update()
    {
        if (GameManager.Instance.PauseGame())
        {
            return;
        }
        CheckCombarInput();
        CheckAttack();
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
            Debug.Log("Attacking");
            // Enemy attack
            coll.transform.SendMessage("IsDamaged", playerData.damage);
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
            //anim.SetBool("isDamaged", true);
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
        GameManager.Instance.HP_Silder.value = HP;
        GameManager.Instance.HP_Text.text = HP.ToString() + "/100";
        Die();
    }

    public void Die()
    {
        if(HP <= 0)
        {
            Destroy(this.gameObject);
            GameManager.Instance.GameOver();
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
