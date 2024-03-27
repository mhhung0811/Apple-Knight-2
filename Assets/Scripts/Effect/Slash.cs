using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CanFlip(float facingDirection)
    {
        if (facingDirection == 1)
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
    public void StartSlash()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isSlash", true);
    }
    public void EndSlash()
    {
        anim.SetBool("isSlash", false);
        EffectManager.Instance.Return(this.gameObject);
    }
}
