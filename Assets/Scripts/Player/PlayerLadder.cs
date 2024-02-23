using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadder : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private GameObject currentLadder;

    private Rigidbody2D myRb;
    private float moveDirection;
    private float curGravity;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InGameManager.Instance.PauseGame())
        {
            return;
        }
        moveDirection = Input.GetAxisRaw("Vertical");
        if (moveDirection != 0)
        {
            if (currentLadder != null)
            {
                myRb.gravityScale = 0;
                myRb.velocity = new Vector2(myRb.velocity.x, moveDirection * speed);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentLadder = collision.gameObject;
        curGravity = myRb.gravityScale;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        currentLadder = null;
    }
}
