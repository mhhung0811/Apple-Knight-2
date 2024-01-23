using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour, IInteractable
{
    private bool isOpen;
    private Animator anim;

    void Start()
    {
        // Close is Default
        isOpen = false;
        anim = GetComponent<Animator>();
        anim.SetBool("isOpen", isOpen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Open()
    {
        if (isOpen) return;
        isOpen = true;
        anim.SetBool("isOpen", isOpen);
        anim.SetBool("isActive", true);
    }
    private void Close()
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
    public void InteractOn()
    {
        Open();
    }
    public void InteractOff()
    {
        Close();
    }
}
