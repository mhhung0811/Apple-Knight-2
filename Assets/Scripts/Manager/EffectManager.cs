using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager _instance;
    public static EffectManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<EffectManager>();
            }
            return _instance;
        }
    }

    [SerializeField]
    private Queue<GameObject> activeEffectDust;
    [SerializeField]
    private GameObject effectDustPrefabs;
    private int effectDustPrepare;
    void Start()
    {
        activeEffectDust = new Queue<GameObject>();
        effectDustPrepare = 3;
        Prepare();
    }

    void Update()
    {
        
    }

    public void Prepare()
    {
        for(int i = 0; i < effectDustPrepare; i++)
        {
            GameObject effect = Instantiate(effectDustPrefabs, transform);
            effect.gameObject.SetActive(false);
            activeEffectDust.Enqueue(effect);
        }
    }

    public GameObject Take()
    {
        if(activeEffectDust.Count <= 0)
        {
            Prepare();
        }
        GameObject effect = this.activeEffectDust.Dequeue();
        effect.gameObject.SetActive(true);
        return effect;
    }

    public void Return(GameObject effect)
    {
        this.activeEffectDust.Enqueue(effect);
        effect.SetActive(false);
    }
}
