using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour, ISaveable
{
    [SerializeField]
    private bool combatEnable;
    [SerializeField]
    private bool hitBoxAppearance;
    [SerializeField]
    private float inputTimer, combatTime, attackRadius;
    [SerializeField]
    private Transform attackHitBoxPos;
    [SerializeField]
    private LayerMask WhatIsDamageable;

    [SerializeField]
    private PlayerData playerData;

    private bool gotInput;

    private int countAttack;

    private float lastInputTime;

    public float PercentDamage;

    [SerializeField]
    private PlayerAnimation animCtrl;
    [SerializeField]
    private PlayerEffect effCtrl;

    private Rigidbody2D myRb;

    private float HP;
    private float MaxHP;
    private float HPEachSecond;

    private void Start()
    {
        myRb = GetComponent<Rigidbody2D>();

        lastInputTime = Mathf.NegativeInfinity;
        countAttack = 0;
        
        MaxHP = HP = playerData.maxHP;
        HPEachSecond = 0;
        PercentDamage = 1;

        // Save
        SaveSystem.onSave += Save;
        // Load
        SaveSystem.onLoad += Load;
    }
    private void Update()
    {
        if (InGameManager.Instance.PauseGame())
        {
            return;
        }
        CheckCombarInput();
        CheckAttack();
    }
    public void HoiHP(float hPEachSecond)
    {
        HPEachSecond = hPEachSecond;
        InvokeRepeating("IncreaseHP", 1f, 1f);
    }
    private void IncreaseHP()
    {
        HP+= HPEachSecond;

        if(HP > MaxHP)
        {
            HP = MaxHP;
        }
        UIManager.Instance.SetHPUi(HP, MaxHP);
    }
    public void TangHP(float maxHp)
    {
        MaxHP = maxHp;
        HP += 50;
        if(HP > MaxHP)
        {
            HP = MaxHP;
        }
        UIManager.Instance.SetHPUi(HP, MaxHP);
    }
    private void CheckCombarInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //AudioManager.Instance.PlaySound("Attack");
            if (combatEnable)
            {
                // Atempt combat
                gotInput = true;
                //lastInputTime = Time.time;
            }
        }
        // Last input time over combat time -> reset combat
        if (lastInputTime < Time.time - combatTime)
        {
            Debug.Log("Reset");
            countAttack = 0;
            animCtrl.StartAttack(-1);
            effCtrl.StartAttack(-1);
        }
    }
    private void CheckAttack()
    {
        if (Time.time < lastInputTime + inputTimer)
        {
            // Wait for new input
            gotInput = false;
        }
        if (gotInput)
        {
            // Apply input
            gotInput = false;
            lastInputTime = Time.time;
            animCtrl.StartAttack(countAttack);
            effCtrl.StartAttack(countAttack);
            CheckAttackHitBox();
            if (countAttack == 0)
            {
                countAttack++;
            }
            else
            {
                countAttack = 0;
            }
        }
        
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackHitBoxPos.position, attackRadius, WhatIsDamageable);
        if (detectedObjects == null) return;
        foreach (Collider2D coll in detectedObjects)
        {
            // Enemy attack
            if(coll.gameObject.CompareTag("Enemy") || coll.gameObject.CompareTag("Boss"))
            {
                coll.transform.SendMessage("IsDamaged", playerData.damage*PercentDamage);
            }
            // Trigger interactable object
            if (coll.gameObject.CompareTag("Interactable Object"))
            {
                coll.gameObject.GetComponent<IInteractable>().InteractOn();
            }
            // Instantiate hit particle
        }
    }

    public void TakeDamage(float damaged, GameObject enemy, float knockback)
    {
        if (damaged > 0)
        {
            HP -= damaged;
            animCtrl.StartDamaged();
        }

        float temp = transform.position.y - enemy.transform.position.y;
        if (temp >= 0)
        {
            myRb.velocity = new Vector2(myRb.velocity.x, knockback);
        }
        else
        {
            myRb.velocity = new Vector2(myRb.velocity.x, -knockback);
        }
        UIManager.Instance.SetHPUi(HP,MaxHP);
        Die();
    }

    public void Die()
    {
        if(HP <= 0)
        {
            Destroy(this.gameObject);
            InGameManager.Instance.GameOver();
        }
    }
    private void OnDrawGizmos()
    {
        if(hitBoxAppearance)
        {
            Gizmos.DrawSphere(attackHitBoxPos.position, attackRadius);
        }
    }
    //UI button
    public void ButtonAttack()
    {
        AudioManager.Instance.PlaySound("Attack");
        if (combatEnable)
        {
            // Atempt combat
            gotInput = true;
            //lastInputTime = Time.time;
        }
    }

    #region ISaveable
    public void Save()
    {
        GameManager.Instance.GameData.Hp = HP;
        GameManager.Instance.GameData.MaxHp = MaxHP;
    }
    public void Load()
    {
        MaxHP = GameManager.Instance.GameData.MaxHp;
        HP = GameManager.Instance.GameData.Hp;
    }
    #endregion
}
