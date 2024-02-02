using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBehavior : MonoBehaviour
{
    [SerializeField]
    private Collider2D hitBox;
    [SerializeField]
    private float force;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Jumping");
            collision.gameObject.GetComponent<PlayerCombatController>().TakeDamage(0, gameObject, force);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Jumping");
            collision.gameObject.GetComponent<PlayerCombatController>().TakeDamage(0, gameObject, force);
        }
    }
}
