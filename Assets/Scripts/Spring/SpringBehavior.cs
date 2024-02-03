using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBehavior : MonoBehaviour
{
    [SerializeField]
    private float force;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void ColliderDetected(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Jumping");
            collision.gameObject.GetComponent<PlayerCombatController>().TakeDamage(0, gameObject, force);
        }
    }
}
