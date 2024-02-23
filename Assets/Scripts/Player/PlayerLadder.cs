using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadder : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Transform ground;

    private bool isActive;
    private bool isFounded;

    private Collider2D currentCollider;
    private Collider2D playerCollider;

    private Rigidbody2D myRb;
    private float moveDirection;
    private float curGravity;

    void Start()
    {
        isActive = false;
        isFounded = false;
        playerCollider = GetComponent<BoxCollider2D>();
        myRb = GetComponent<Rigidbody2D>();
        curGravity = myRb.gravityScale;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (InGameManager.Instance.PauseGame())
        {
            return;
        }

        isFounded = false;
        moveDirection = Input.GetAxisRaw("Vertical");

        Collider2D[] colls = Physics2D.OverlapBoxAll(ground.position, new Vector2(1, 0.2f), 0);
        foreach (Collider2D coll in colls)
        {
            if (coll.CompareTag("Ladder"))
            {
                currentCollider = coll;
                isFounded = true;
                break;
            }
        }

        if (!isFounded)
        {
            currentCollider = null;
            isActive = false;
            myRb.gravityScale = curGravity;
        }
        if (isFounded && moveDirection != 0)
        {
            isActive = true;
            myRb.gravityScale = 0;

        }

        if (isActive)
        {
            myRb.velocity = new Vector2(myRb.velocity.x, moveDirection * speed);
        }
    }

    public IEnumerator DisableCollision()
    {
        if (currentCollider != null)
        {
            Collider2D temp = currentCollider;
            Debug.Log("isWorking");
            isActive = false;
            myRb.gravityScale = curGravity;
            Physics2D.IgnoreCollision(playerCollider, temp);
            yield return new WaitForSeconds(0.5f);
            isActive = true;
            myRb.gravityScale = 0;
            Physics2D.IgnoreCollision(playerCollider, temp, false);
        }
    }
}
