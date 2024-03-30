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
    private List<GameObject> flyingBats;
    void Start()
    {
        LoadEnemy();
    }

    void Update()
    {
        
    }
    public void LoadEnemy()
    {
        int lengthFlyingBat = flyingBats.Count;
        for(int i = 0;i < lengthFlyingBat; i++)
        {
            flyingBats[i].GetComponent<BatMonster>().SetID(i);
        }
    }
    public void SetAactiveEnemyDie(int id)
    {
        flyingBats[id].SetActive(false);
    }
}
