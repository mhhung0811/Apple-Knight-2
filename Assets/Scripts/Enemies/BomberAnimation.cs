using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberAnimation : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    private Enemy3 mainCtrl;
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
    }
    public void Attacking()
    {
        mainCtrl.Attacking();
    }
    public void EndAttack()
    {
        Debug.Log("finish");
        anim.SetBool("isAttack", false);
        mainCtrl.FinishAttack1();
    }
    public void StartCast(float facing)
    {
        anim.SetBool("isCast", true);
    }
    public void Casting()
    {
        mainCtrl.Casting();
    }
    public void EndCast()
    {
        anim.SetBool("isCast", false);
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
