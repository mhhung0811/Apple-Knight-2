using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Divine : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private float damage;
    void Start()
    {
        damage = 50;
    }

    void Update()
    {
        if (InGameManager.Instance.PauseGame())
        {
            return;
        }
        this.transform.position = player.transform.position;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            collision.transform.SendMessage("IsDamaged", damage);
        }
    }
}
