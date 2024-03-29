using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMageAnimation : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    private BossState mainCtrl;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMultiFireBall()
    {
        anim.SetBool("isFireBall", true);
    }
    public void EndMultiFireBall()
    {
        anim.SetBool("isFireBall", false);
    }

    public void StartTele()
    {
        anim.SetBool("isTele", true);
    }
    public void Teleporting()
    {
        MagicEffect.PlayEffect(MAGICEFFECT.Rotate, this.transform.position.x, this.transform.position.y + 2f);
    }
    public void EndTele()
    {
        anim.SetBool("isTele", false);
    }

    public void StartDash()
    {
        anim.SetBool("isDash", true);
    }
    public void EndDash()
    {
        anim.SetBool("isDash", false);
    }
    public void StartSpawn()
    {
        anim.SetBool("isSpawnEnemies", true);
    }
    public void EndSpawn()
    {
        anim.SetBool("isSpawnEnemies", false);
    }
    public void StartBomb()
    {
        anim.SetBool("isBomb", true);
    }
    public void IsBombing()
    {
        mainCtrl.Bombing();
    }
    public void EndBomb()
    {
        anim.SetBool("isBomb", false);
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
