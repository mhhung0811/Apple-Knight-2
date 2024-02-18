using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Divine : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }
    public void Beng()
    {
        anim.SetBool("IsBeng", true);
    }
}
