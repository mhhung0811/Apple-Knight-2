using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfigs", menuName = "ScriptableObject/PlayerConfigs")]
public class PlayerData : ScriptableObject
{
    public float maxHP = 100;
    public float damage = 10;
    public float moveSpeed = 10f;
    public float jumpForce = 16f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlidingSpeed;
    public float dashTime = 0.2f;
    public float dashSpeed = 35f;
    public float dashCoolDown = 0.25f;
    public float jumpStompForce = 50f;
    public float HitBoxAttack = 1.5f;
    public float HitBoxJumpForce = 2f;
    public int amountOfJump = 1;
}
