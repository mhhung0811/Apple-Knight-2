using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private int countAttack;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("countAttack", -1);
        countAttack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartAttack(int num)
    {
        anim.SetInteger("countAttack", num);
    }

    public void StartRun()
    {
        anim.SetBool("isRunning", true);
    }
    public void StartJump()
    {
        anim.SetBool("isJumping", true);
    }
    public void StartDash()
    {
        anim.SetBool("isDashing", true);
    }
    public void StartWallSlide()
    {
        anim.SetBool("isWallSliding", true);
    }
    public void StartDamaged()
    {
        anim.SetBool("isDamaged", true);
    }

    public void FinishAttack()
    {
        anim.SetInteger("countAttack", -1);
    }
    public void FinishRun()
    {
        anim.SetBool("isRunning", false);
    }
    public void FinishJump()
    {
        anim.SetBool("isJumping", false);
    }
    public void FinishDash()
    {
        anim.SetBool("isDashing", false);
    }
    public void FinishWallSlide()
    {
        anim.SetBool("isWallSliding", false);
    }
    public void FinishDamaged()
    {
        anim.SetBool("isDamaged", false);
    }
}
