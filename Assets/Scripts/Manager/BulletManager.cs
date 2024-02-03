using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private static BulletManager _instance;
    public static BulletManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<BulletManager>();
            }
            return _instance;
        }
    }

    private Queue<GameObject> _activeBomb;
    [SerializeField]
    private GameObject Bomb;
    private int BombPrepare;

    private Queue<GameObject> _activeDarts;
    [SerializeField]
    private GameObject Darts;
    private int DartsPrepare;

    private Queue<GameObject> _activeFireBall;
    [SerializeField]
    private GameObject FireBall;
    private int FireBallPrepare;

    private void Awake()
    {
        if (_instance == null)
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
        FireBallPrepare = DartsPrepare = BombPrepare = 3;
        _activeBomb = new Queue<GameObject>();
        _activeDarts = new Queue<GameObject>();
        _activeFireBall = new Queue<GameObject>();
    }
    void Update()
    {
        
    }

    public void PrepareBomb()
    {
        for(int i= 0; i< BombPrepare;i++)
        {
            GameObject bomb = Instantiate(Bomb, transform);
            bomb.gameObject.SetActive(false);
            _activeBomb.Enqueue(bomb);
        }
    }
    public GameObject TakeBomb()
    {
        if(_activeBomb.Count <= 0)
        {
            PrepareBomb();
        }
        GameObject bomb = this._activeBomb.Dequeue();
        bomb.gameObject.SetActive(true);
        return bomb;
    }
    public void ReturnBomb(GameObject bomb)
    {
        this._activeBomb.Enqueue(bomb);
        bomb.SetActive(false);
    }

    public void PrepareDarts()
    {
        for (int i= 0;i< DartsPrepare;i++)
        {
            GameObject darts = Instantiate(Darts, transform);
            darts.gameObject.SetActive(false);
            this._activeDarts.Enqueue(darts);
        }
    }
    public GameObject TakeDarts()
    {
        if( _activeDarts.Count <= 0)
        {
            PrepareDarts();
        }
        GameObject darts = this._activeDarts.Dequeue();
        darts.gameObject.SetActive(true);
        return darts;
    }
    public void ReturnDarts(GameObject darts)
    {
        this._activeDarts.Enqueue(darts);
        darts.SetActive(false);
    }

    public void PrepareFireBall()
    {
        for(int i= 0; i< FireBallPrepare; i++)
        {
            GameObject fireball = Instantiate(FireBall, transform);
            fireball.gameObject.SetActive(false);
            this._activeFireBall.Enqueue(fireball);
        }
    }
    public GameObject TakeFireBall()
    {
        if(_activeFireBall.Count <= 0)
        {
            PrepareFireBall();
        }
        GameObject fireball = this._activeFireBall.Dequeue();
        fireball.gameObject.SetActive(true);
        return fireball;
    }
    public void ReturnFireBall(GameObject fireball)
    {
        this._activeFireBall.Enqueue(fireball);
        fireball.SetActive(false);
    }
}
