using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darts : MonoBehaviour
{
    private float speed;
    private float damage;
    private float facingDirection;
    Rigidbody2D myRb;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        speed = 20;
        damage = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.PauseGame())
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
        if (facing == 1)
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.transform.SendMessage("IsDamaged", damage);
            BulletManager.Instance.ReturnDarts(this.gameObject);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            BulletManager.Instance.ReturnDarts(this.gameObject);
        }
    }
}
