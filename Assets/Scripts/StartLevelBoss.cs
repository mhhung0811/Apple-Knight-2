using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevelBoss : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                InGameManager.Instance.StartLevelBoss();
            }
        }
    }
}
