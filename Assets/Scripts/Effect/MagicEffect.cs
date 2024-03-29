using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicEffect : MonoBehaviour
{
    private Animator anim;
    public static void PlayEffect(MAGICEFFECT type, float x, float y)
    {
        GameObject effect;
        effect = EffectManager.Instance.Take(EFFECTTYPE.MagicEffect);
        effect.transform.position = new Vector3(x, y, 0);
        MagicEffect e = effect.GetComponent<MagicEffect>();
        switch (type)
        {
            case MAGICEFFECT.SpawnDown:
                e.StartSpawnDown();
                break;
            case MAGICEFFECT.SpawnDownReverse:
                e.StartSpawnDownReverse();
                break;
            case MAGICEFFECT.Rotate:
                e.StartRotate();
                break;
            case MAGICEFFECT.RotateReverse:
                e.StartRotateReverse();
                break;
            case MAGICEFFECT.Rotate2:
                e.StartRotate2();
                break;
            case MAGICEFFECT.EnergyExplode:
                e.StartEnergyExplode();
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void StartSpawnDown()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isSpawnDown", true);
    }
    private void StartSpawnDownReverse()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isSpawnDownReverse", true);
    }
    public void EndSpawnDown()
    {
        anim.SetBool("isSpawnDown", false);
        anim.SetBool("isSpawnDownReverse", false);
        EffectManager.Instance.Return(this.gameObject);
    }
    private void StartRotate()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isRotate", true);
    }
    private void StartRotateReverse()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isRotateReverse", true);
    }
    public void EndRotate()
    {
        anim.SetBool("isRotate", false);
        anim.SetBool("isRotateReverse", false);
        EffectManager.Instance.Return(this.gameObject);
    }
    private void StartRotate2()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isRotate2", true);
    }
    public void EndRotate2()
    {
        anim.SetBool("isRotate2", false);
        EffectManager.Instance.Return(this.gameObject);
    }
    private void StartEnergyExplode()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isEnergyExplode", true);
    }
    public void EndEnergyExplode()
    {
        anim.SetBool("isEnergyExplode", false);
        EffectManager.Instance.Return(this.gameObject);
    }
}
