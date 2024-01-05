using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] 
    private float MaxHP;
    private float HP;

    Animator anim;
    Rigidbody2D myRb;
    // Start is called before the first frame update
    void Start()
    {
        MaxHP = 1000;
        HP = MaxHP;
        anim = GetComponent<Animator>();
        myRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void IsDamaged(float damage)
    {
        HP -= damage;
        anim.SetBool("isDamaging", true);

        // Knockback
        myRb.AddForce(new Vector2(200,100));
    }
    public void FinishDamaged()
    {
        anim.SetBool("isDamaging", false);
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
