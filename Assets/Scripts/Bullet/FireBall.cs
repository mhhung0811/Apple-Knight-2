using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private float speed;
    private float damage;
    private float facingDirection;
    Rigidbody2D myRb;
    public LayerMask whatIsGround;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        speed = 20;
        damage = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (InGameManager.Instance.PauseGame())
        {
            return;
        }
        Move();
    }

    public void Move()
    {
        myRb.velocity = new Vector2(speed * facingDirection, 0);
    }

    public void SetUp(float facing)
    {
        if(facing == 1)
        {
            facingDirection = 1;
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            facingDirection = -1;
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            player.GetComponent<PlayerCombatController>().TakeDamage(damage, gameObject, 0);
            BulletManager.Instance.ReturnFireBall(this.gameObject);
        }
        if (whatIsGround == (whatIsGround | 1 << collision.gameObject.layer))
        {
            BulletManager.Instance.ReturnFireBall(this.gameObject);
        }
    }
}
