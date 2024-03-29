using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDown : MonoBehaviour
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
    public void StartSpawn()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isSpawn", true);
    }
    public void EndSpawn()
    {
        anim.SetBool("isSpawn", false);
        EffectManager.Instance.Return(this.gameObject);
    }
}
