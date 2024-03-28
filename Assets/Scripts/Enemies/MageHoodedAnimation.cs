using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageHoodedAnimation : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    private Enemy2 mainCtrl;
    [SerializeField]
    private Transform attackHitBox;
    private float facingDirection;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAttack(float facing)
    {
        anim.SetBool("isAttack", true);
        facingDirection = facing;
    }
    public void Attacking()
    {
        mainCtrl.Attacking();
    }
    public void EndAttack()
    {
        anim.SetBool("isAttack", false);
        mainCtrl.FinishAttack1();
    }
    public void StartCast(float facing)
    {
        anim.SetBool("isCast", true);
        facingDirection = facing;
    }
    public void Casting()
    {
        mainCtrl.Casting();
    }
    public void EndCast()
    {
        anim.SetBool("isCast", false);
    }
    public void StartWalk()
    {
        anim.SetBool("isWalk", true);
    }
    public void EndWalk()
    {
        anim.SetBool("isWalk", false);
    }
    public void StartDamaged()
    {
        anim.SetBool("isDamaged", true);
    }
    public void EndDamaged()
    {
        anim.SetBool("isDamaged", false);
        mainCtrl.FinishDamaged();
    }
}
