using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBoss : MonoBehaviour
{
    private float damage;
    private float explodeRadius;
    public float timeExplode;

    public LayerMask whatIsPlayer;
    public LayerMask whatIsGround;
    void Start()
    {
        damage = 10;
        explodeRadius = 2f;
        timeExplode = 1f;
    }


    void Update()
    {
        if (InGameManager.Instance.PauseGame())
        {
            return;
        }

        Explode();
    }

    private void Explode()
    {
        timeExplode -= Time.deltaTime;
        if(timeExplode <= 0)
        {
            AudioManager.Instance.PlaySound("Explosion");
            //Animation Effect Explode
            GameObject effectExplode = EffectManager.Instance.Take();
            effectExplode.transform.position = this.transform.position;
            Dust dust = effectExplode.GetComponent<Dust>();
            dust.StartAnimExplode();
            CheckHitBoxExplode();
            BulletManager.Instance.ReturnBombBoss(this.gameObject);
        }
    }

    private void CheckHitBoxExplode()
    {
        Collider2D collider = Physics2D.OverlapCircle(this.transform.position, explodeRadius, whatIsPlayer);
        if (collider != null)
        {
            GameObject player = collider.gameObject;
            player.GetComponent<PlayerCombatController>().TakeDamage(damage, gameObject, 15);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, explodeRadius);
    }
}
