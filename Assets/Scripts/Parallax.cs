using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float lengthX, lengthY, startPosX, startPosY;
    [SerializeField]
    private GameObject cam;
    [SerializeField]
    private SpriteRenderer image;
    [SerializeField]
    [Range(0,1)]
    public float parallaxEffectX;
    [SerializeField]
    [Range(0,1)]
    public float parallaxEffectY;
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        lengthX = image.bounds.size.x;
        lengthY = image.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        float tempX = (cam.transform.position.x * (1 - parallaxEffectX));
        float tempY = (cam.transform.position.y * (1 - parallaxEffectY));
        float distX = (cam.transform.position.x * parallaxEffectX);
        float distY = (cam.transform.position.y * parallaxEffectY);

        transform.position = Vector3.Lerp(transform.position, new Vector3(startPosX + distX, startPosY + distY, transform.position.z), 0.5f);
        //transform.position = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);

        if (tempX > startPosX + lengthX)
        {
            startPosX += lengthX;
            transform.position = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);
        }
        else if (tempX < startPosX - lengthX)
        {
            startPosX -= lengthX;
            transform.position = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);
        }

        if (tempY > startPosY + lengthY)
        {
            startPosY += lengthY;
            transform.position = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);
        }
        else if (tempY < startPosY - lengthY)
        {
            startPosY -= lengthY;
            transform.position = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);
        }
    }
}
