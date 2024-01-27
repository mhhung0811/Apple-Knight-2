using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed;
    public float yOffset;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 newPos = new Vector3(target.transform.position.x, target.transform.position.y + yOffset, -10);
            this.transform.position = Vector3.Slerp(transform.position, newPos, followSpeed*Time.deltaTime);
        }
    }
}
