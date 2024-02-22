using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoaDon : MonoBehaviour
{
    private float speed;
    private float damage;
    private float facingDirection;
    private float timeSpawn;
    private Rigidbody2D myRb;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        speed = 10;
        damage = 90;
    }

    // Update is called once per frame
    void Update()
    {
        if (InGameManager.Instance.PauseGame())
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

    public void SetUp(float facing)
    {
        timeSpawn = 0f;
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

    private void CanDestroy()
    {
        timeSpawn += Time.deltaTime;
        if(timeSpawn > 5f)
        {
            BulletManager.Instance.ReturnHoaDon(this.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.transform.SendMessage("IsDamaged", damage);
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            collision.transform.SendMessage("IsDamaged", damage);
            AudioManager.Instance.PlaySound("Explosion");

            GameObject effectExplode = EffectManager.Instance.Take();
            effectExplode.transform.position = this.transform.position;
            Dust dust = effectExplode.GetComponent<Dust>();
            dust.StartAnimExplode();
            BulletManager.Instance.ReturnHoaDon(this.gameObject);
        }
    }
}
