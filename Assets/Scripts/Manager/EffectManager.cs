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

    // 0 is Dust
    [SerializeField]
    private Queue<GameObject> activeEffectDust;
    [SerializeField]
    private GameObject effectDustPrefabs;
    private int effectDustPrepare;

    // 1 is Slash
    private Queue<GameObject> activeSlashEffect;
    [SerializeField]
    private GameObject slashEffectPrefabs;
    private int slashEffectPrepare;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        activeEffectDust = new Queue<GameObject>();
        effectDustPrepare = 3;
        activeSlashEffect = new Queue<GameObject>();
        slashEffectPrepare = 10;
        Prepare(0);
        Prepare(1);
    }

    void Update()
    {
        
    }

    public void Prepare(int obj)
    {
        switch (obj)
        {
            case 0:
                for (int i = 0; i < effectDustPrepare; i++)
                {
                    GameObject effect = Instantiate(effectDustPrefabs, transform);
                    effect.gameObject.SetActive(false);
                    activeEffectDust.Enqueue(effect);
                }
                break;
            case 1:
                for (int i = 0; i < slashEffectPrepare; i++)
                {
                    GameObject effect = Instantiate(slashEffectPrefabs, transform);
                    effect.gameObject.SetActive(false);
                    activeSlashEffect.Enqueue(effect);
                }
                break;
        }
    }

    public GameObject Take(int obj)
    {
        GameObject effect = null;
        switch (obj)
        {
            case 0:
                if (activeEffectDust.Count <= 0)
                {
                    Prepare(obj);
                }
                effect = this.activeEffectDust.Dequeue();
                effect.gameObject.SetActive(true);
                break;
            case 1:
                if (activeSlashEffect.Count <= 0)
                {
                    Prepare(obj);
                }
                effect = this.activeSlashEffect.Dequeue();
                effect.gameObject.SetActive(true);
                break;
        }

        return effect;
    }

    public void Return(GameObject effect)
    {
        if (effect.TryGetComponent<Dust>(out Dust dust))
        {
            this.activeEffectDust.Enqueue(effect);
            effect.SetActive(false);
        }
        if (effect.TryGetComponent<Slash>(out Slash slash))
        {
            this.activeSlashEffect.Enqueue(effect);
            effect.SetActive(false);
        }
    }
}
