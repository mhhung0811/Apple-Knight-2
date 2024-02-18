using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentoryu: MonoBehaviour
{
    private float speed;
    private int facingDirection;
    private float damage;
    private float timeSpawn;
    private int indexSentoryu;
    private Rigidbody2D myRb;
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        speed = 30;
        damage = 30;
    }

    void Update()
    {
        if (GameManager.Instance.PauseGame())
        {
            return;
        }
        Move();
        CanDestroy();
    }

    public void Move()
    {
        myRb.velocity = new Vector2(speed * facingDirection, 0);
    }

    public void SetUp(float facing, int indexStr)
    {
        timeSpawn = 0;
        indexSentoryu = indexStr;
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
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            collision.transform.SendMessage("IsDamaged", damage);
        }
    }
    public void CanDestroy()
    {
        timeSpawn += Time.deltaTime;
        if(timeSpawn > 5)
        {
            switch (indexSentoryu)
            {
                case 1:
                    BulletManager.Instance.ReturnSentoryu1(this.gameObject);
                    break;
                case 2:
                    BulletManager.Instance.ReturnSentoryu2(this.gameObject);
                    break;
                case 3:
                    BulletManager.Instance.ReturnSentoryu3(this.gameObject);
                    break;
            }
        }
    }
}
