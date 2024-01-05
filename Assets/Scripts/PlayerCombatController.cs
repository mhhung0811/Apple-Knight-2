using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField]
    private bool combatEnable;
    [SerializeField]
    private float inputTimer, attackRadius, attackDamage;
    [SerializeField]
    private Transform attackHitBoxPos;
    [SerializeField]
    private LayerMask WhatIsDamageable;

    private bool gotInput, isAttacking;

    private float lastInputTime;

    private Animator anim;

    private void Start()
    {
        lastInputTime = Mathf.NegativeInfinity;
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
    }
    private void CheckAttack()
    {
        if (gotInput)
        {
            if (!isAttacking)
            {
                Debug.Log("attack");
                gotInput = false;
                isAttacking = true;
                anim.SetBool("isAttacking", isAttacking);
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
    private void FinishAttack()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(attackHitBoxPos.position, attackRadius);
    //}
}
