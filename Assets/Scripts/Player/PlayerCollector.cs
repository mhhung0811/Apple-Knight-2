using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Collectable Object"))
        {
            //Audio Collect coin
            AudioManager.Instance.PlaySound("CollectCoin");
            // Collect coin
            //GameManager.Instance.CollectCoin();
            // Destroy Coin
            Destroy(collision.gameObject);
            // Add coin ...
        }
    }
}
