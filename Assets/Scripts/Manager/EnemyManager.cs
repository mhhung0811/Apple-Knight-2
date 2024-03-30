using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager Instance
    {
        get
        {
            if(_instance == null)
                _instance = FindObjectOfType<EnemyManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null)
            _instance = this;
        else
            Destroy(_instance);
    }

    [SerializeField]
    private List<GameObject> enemy;
    void Start()
    {
        LoadEnemy();
    }

    void Update()
    {
        
    }
    public void LoadEnemy()
    {
        int lengthFlyingBat = enemy.Count;
        for(int i = 0;i < lengthFlyingBat; i++)
        {
            enemy[i].GetComponent<BaseEnemy>().SetID(i);
        }
    }
    public void SetAactiveEnemyDie(int id)
    {
        enemy[id].SetActive(false);
    }
}
