using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    private bool isOpen;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        anim = GetComponent<Animator>();
        anim.SetBool("isOpen", isOpen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Open()
    {
        if (isOpen) return;
        isOpen = true;
        anim.SetBool("isOpen", isOpen);
        anim.SetBool("isActive", true);
    }
    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;
        anim.SetBool("isOpen", isOpen);
        anim.SetBool("isActive", true);
    }
    public void endActive()
    {
        anim.SetBool("isActive", false);
    }
}
