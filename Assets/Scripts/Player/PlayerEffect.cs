using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    public void StartQ3()
    {
        anim.SetBool("IsBeng", true);
    }

    public void FinishQ3()
    {
        anim.SetBool("IsBeng", false);
    }

    public void StartAttack1()
    {
        anim.SetBool("Attack1", true);
    }

    public void FinishAttack1()
    {
        anim.SetBool("Attack1", false);
    }

    public void StartAttack2()
    {
        anim.SetBool("Attack2", true);
    }

    public void FinishAttack2()
    {
        anim.SetBool("Attack2", false);
    }
}
