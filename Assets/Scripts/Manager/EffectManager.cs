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

    // 2 is Magic Effect
    private Queue<GameObject> activeMagicEffect;
    [SerializeField]
    private GameObject magicEffectPrefabs;
    private int magicEffectPrepare;
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
        slashEffectPrepare = 15;
        activeMagicEffect = new Queue<GameObject>();
        magicEffectPrepare = 10;

        Prepare(EFFECTTYPE.Dust);
        Prepare(EFFECTTYPE.Slash);
        Prepare(EFFECTTYPE.MagicEffect);
    }

    void Update()
    {
        
    }

    public void Prepare(EFFECTTYPE type)
    {
        switch (type)
        {
            case EFFECTTYPE.Dust:
                for (int i = 0; i < effectDustPrepare; i++)
                {
                    GameObject effect = Instantiate(effectDustPrefabs, transform);
                    effect.gameObject.SetActive(false);
                    activeEffectDust.Enqueue(effect);
                }
                break;
            case EFFECTTYPE.Slash:
                for (int i = 0; i < slashEffectPrepare; i++)
                {
                    GameObject effect = Instantiate(slashEffectPrefabs, transform);
                    effect.gameObject.SetActive(false);
                    activeSlashEffect.Enqueue(effect);
                }
                break;
            case EFFECTTYPE.MagicEffect:
                for (int i = 0; i < magicEffectPrepare; i++)
                {
                    GameObject effect = Instantiate(magicEffectPrefabs, transform);
                    effect.gameObject.SetActive(false);
                    activeMagicEffect.Enqueue(effect);
                }
                break;
        }
    }

    public GameObject Take(EFFECTTYPE type)
    {
        GameObject effect = null;
        switch (type)
        {
            case EFFECTTYPE.Dust:
                if (activeEffectDust.Count <= 0)
                {
                    Prepare(EFFECTTYPE.Dust);
                }
                effect = this.activeEffectDust.Dequeue();
                effect.gameObject.SetActive(true);
                break;
            case EFFECTTYPE.Slash:
                if (activeSlashEffect.Count <= 0)
                {
                    Prepare(EFFECTTYPE.Slash);
                }
                effect = this.activeSlashEffect.Dequeue();
                effect.gameObject.SetActive(true);
                break;
            case EFFECTTYPE.MagicEffect:
                if (activeMagicEffect.Count <= 0)
                {
                    Prepare(EFFECTTYPE.MagicEffect);
                }
                effect = this.activeMagicEffect.Dequeue();
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
        if (effect.TryGetComponent<MagicEffect>(out MagicEffect magic))
        {
            this.activeMagicEffect.Enqueue(effect);
            effect.SetActive(false);
        }
    }
}
