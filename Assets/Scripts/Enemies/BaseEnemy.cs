using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    //protected EnemyData enemyData;
    protected bool isFacingLeft;
    protected bool canMove;// Di chuyen
    protected bool isDetectInHitBox;
    protected float facingDirection;
    protected int id;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public virtual void SetID(int id)
    {
        this.id = id;
    }
    public virtual void InitializedEnemy() 
    {
        canMove = false;
    }
    public virtual void Move() { }
    public virtual void IsDamaged(float damage) { }
    public virtual void FinishDamaged() { }
    public virtual void DetectPlayer() { }
    public virtual void DealDamage() { }
}
