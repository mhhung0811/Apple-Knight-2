using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    protected EnemyData enemyData;
    protected bool isFacingLeft;
    protected bool canMove;// Di chuyen
    protected bool isDetectInHitBox;
    protected float facingDirection;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public virtual void InitializedEnemy() 
    {
        enemyData = new EnemyData();
        enemyData.maxHP = 100;
        enemyData.damage = 10;
        enemyData.HP = enemyData.maxHP;
        enemyData.speed = 5;
        enemyData.detectionRange = 10;
        canMove = false;
    }
    public virtual void Move() { }
    public virtual void IsDamaged(float damage) { }
    public virtual void FinishDamaged() { }
    public virtual void DetectPlayer() { }
    public virtual void DealDamage() { }
}
