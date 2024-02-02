using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dust : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        //AnimJump();
    }

    void Update()
    {
        
    }

    public void StartAnimJump()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("IsJump", true);
    }

    public void FinishAnimJump()
    {
        anim.SetBool("IsJump", false);
        EffectManager.Instance.Return(this.gameObject);
    }

    public void StartAnimDustLand()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("IsLand", true);
    }

    public void FinishAnimDustLand()
    {
        anim.SetBool("IsLand", false);
        EffectManager.Instance.Return(this.gameObject);
    }

    public void StartAnimDash()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("IsDash", true);
    }

    public void FinishAnimDash()
    {
        anim.SetBool("IsDash", false);
        EffectManager.Instance.Return(this.gameObject);
    }

    public void StartAnimExplode()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("IsExplode", true);
    }

    public void FinishAnimExplode()
    {
        anim.SetBool("IsExplode", false);
        EffectManager.Instance.Return(this.gameObject);
    }
}
