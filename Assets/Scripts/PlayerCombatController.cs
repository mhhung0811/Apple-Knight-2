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

    private bool gotInput, isCombat, isAttacking1, isAttacking2;

    private int countAttack;

    private float lastInputTime;

    private Animator anim;

    private void Start()
    {
        lastInputTime = Mathf.NegativeInfinity;
        countAttack = 0;
        isCombat = false;

        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnable);
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
                }

                if (countAttack == 1 && !isAttacking2)
                {
                    // Perform attack 2
                    isAttacking2 = true;
                    anim.SetBool("isAttacking2", isAttacking2);
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
        foreach (Collider2D coll in detectedObjects)
        {
            //coll.transform.parent.SendMessage("Damage", attackDamage);
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
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(attackHitBoxPos.position, attackRadius);
    //}
}
