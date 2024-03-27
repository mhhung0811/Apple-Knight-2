using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimation : MonoBehaviour
{
    private Animator anim;
    [SerializeField] Enemy4 mainCtrl;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartAttack()
    {
        anim.SetBool("isAttack", true);
    }
    public void EndAttack()
    {
        anim.SetBool("isAttack", false);
        mainCtrl.FinishAttack1();
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
