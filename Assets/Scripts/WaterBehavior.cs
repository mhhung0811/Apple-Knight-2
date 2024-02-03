using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehavior : MonoBehaviour
{
    private GameObject currentCharacter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            currentCharacter = collision.gameObject;
            Debug.Log("Damnn!");
            // Deal damage
            currentCharacter.GetComponent<PlayerCombatController>().TakeDamage(1, this.gameObject, 16);
            // Character to immortal state
        }
    }
}
