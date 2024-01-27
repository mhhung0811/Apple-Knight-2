using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfigs", menuName = "ScriptableObject/EnemyConfigs")]
public class EnemyData : ScriptableObject
{
    public float maxHP = 100f;
    public float speed = 3f;
    public float damage = 10f;
    public float detectionRange = 10f;
}
