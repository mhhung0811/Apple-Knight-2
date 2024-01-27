using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField]
    private bool combatEnable;
    [SerializeField]
    private float inputTimer, combatTime, attackRadius, attackDamage;
    [SerializeField]
    private Transform attackHitBoxPos;
    [SerializeField]
    private LayerMask WhatIsDamageable;

    [SerializeField]
    private PlayerData playerData;

    private bool gotInput, isCombat, isAttacking1, isAttacking2;

    private int countAttack;

    private float lastInputTime;

    private Animator anim;

    private Rigidbody2D myRb;

    private float HP;

    private void Start()
    {
        myRb = GetComponent<Rigidbody2D>();

        lastInputTime = Mathf.NegativeInfinity;
        countAttack = 0;
        isCombat = false;

        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnable);
        HP = playerData.maxHP;
    }
    private void Update()
    {
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
                lastInputTime = Time.time;
            }
        }
        // Last input time over combat time -> reset combat
        if (Time.time > lastInputTime + combatTime)
        {
            countAttack = 0;
            anim.SetInteger("countAttack", countAttack);
            isCombat = false;
            anim.SetBool("isCombat", isCombat);
        }
    }
    private void CheckAttack()
    {
        if (gotInput)
        {
            // Start attack
            if (!isCombat)
            {
                // Apply input
                gotInput = false;

                // Start combat
                isCombat = true;
                anim.SetBool("isCombat", isCombat);

                // Perform attack 1
                isAttacking1 = true;
                anim.SetBool("isAttacking1", isAttacking1);
                CheckAttackHitBox();
            }
            // is combating
            else
            {
                // Apply input
                gotInput = false;
                if (countAttack == 0 && !isAttacking1)
                {
                    // Perform attack 1
                    isAttacking1 = true;
                    anim.SetBool("isAttacking1", isAttacking1);
                    CheckAttackHitBox();
                }

                if (countAttack == 1 && !isAttacking2)
                {
                    // Perform attack 2
                    isAttacking2 = true;
                    anim.SetBool("isAttacking2", isAttacking2);
                    CheckAttackHitBox();
                }
            }
        }
        if (Time.time > lastInputTime + inputTimer)
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
            coll.transform.SendMessage("IsDamaged", attackDamage);
            // Trigger interactable object
            if (coll.gameObject.CompareTag("Interactable Object"))
            {

                coll.gameObject.GetComponent<IInteractable>().InteractOn();
            }
            // Instantiate hit particle
        }
    }
    private void FinishAttack1()
    {
        isAttacking1 = false;
        anim.SetBool("isAttacking1", isAttacking1);
        countAttack = 1;
        anim.SetInteger("countAttack", countAttack);
    }
    private void FinishAttack2()
    {
        isAttacking2 = false;
        anim.SetBool("isAttacking2", isAttacking2);
        // Last attack
        countAttack = 0;
        anim.SetInteger("countAttack", countAttack);

    }

    public void TakeDamage(float damaged, GameObject enemy, float knockback)
    {
        HP -= damaged;
        float temp = transform.position.y - enemy.transform.position.y;
        if (temp >= 0)
        {
            myRb.velocity = new Vector2(myRb.velocity.x, knockback);
        }
        else
        {
            myRb.velocity = new Vector2(myRb.velocity.x, -knockback);
        }
        Debug.Log(HP);
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
        Gizmos.DrawSphere(attackHitBoxPos.position, attackRadius);
    }
    //UI button
    public void ButtonAttack()
    {
        AudioManager.Instance.PlaySound("Attack");
        if (combatEnable)
        {
            // Atempt combat
            gotInput = true;
            lastInputTime = Time.time;
        }
    }
}
