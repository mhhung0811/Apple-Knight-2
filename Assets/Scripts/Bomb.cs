using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float speed;
    private Rigidbody2D myRb;
    private float damage;
    void Start()
    {
        damage = 10;
    }

    
    void Update()
    {
        
    }

    public void SetUp(float H, float L, float facing)
    {
        myRb=GetComponent<Rigidbody2D>();
        speed = Mathf.Sqrt(Mathf.Abs((-9.81f * L * L) / (H - L)));
        myRb.velocity = new Vector2(1*facing,1).normalized * speed;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            player.GetComponent<PlayerCombatController>().TakeDamage(damage, gameObject, 0);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
