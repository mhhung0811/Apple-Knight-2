using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfigs", menuName = "ScriptableObject/EnemyConfigs")]
public class EnemyData : ScriptableObject
{
    public float maxHP = 100;
    public float speed = 3;
    public float damage = 10;
    public float detectionRange = 10;
}
