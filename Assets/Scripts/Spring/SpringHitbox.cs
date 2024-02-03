using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.parent.GetComponent<SpringBehavior>().ColliderDetected(collision);
    }
}
