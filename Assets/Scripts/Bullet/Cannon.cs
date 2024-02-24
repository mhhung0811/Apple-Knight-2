using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private float delayTime;
    [SerializeField]
    private float coolDownFire;
    void Start()
    {
        StartFire();
    }

    void Update()
    {
        
    }


    public void Fire()
    {
        GameObject fireBall = BulletManager.Instance.TakeFireBall();
        FireBall fb = fireBall.GetComponent<FireBall>();
        fb.transform.position = this.transform.position;
        fb.transform.rotation = this.transform.rotation;
        fb.SetUpSpecific(this.transform.rotation.eulerAngles.z);
    }

    private void StartFire()
    {
        InvokeRepeating("Fire", delayTime, coolDownFire);
    }
}
