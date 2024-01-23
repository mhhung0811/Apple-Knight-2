using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehavior : MonoBehaviour
{
    private GameObject currentCharacter;
    private BoxCollider2D spikeCollider;

    // Start is called before the first frame update
    void Start()
    {
        spikeCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            currentCharacter = collision.gameObject;
            Debug.Log("Damnn!");
            // Deal damage
            // Character to immortal state
        }
    }
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        currentCharacter = collision.gameObject;
    //        Debug.Log("Damnn!");
    //        // Deal damage
    //        // Character to immortal state
    //    }
    //}
}
