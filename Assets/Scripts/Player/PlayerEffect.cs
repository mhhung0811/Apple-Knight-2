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

    public void StartAttack(int num)
    {
        anim.SetInteger("CountAttack", num);
    }

    public void FinishAttack()
    {
        anim.SetInteger("CountAttack", -1);
    }
}
