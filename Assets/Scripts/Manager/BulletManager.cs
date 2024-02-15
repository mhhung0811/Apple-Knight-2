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

    private Queue<GameObject> _activeFireBallBoss;
    [SerializeField]
    private GameObject FireBallBoss;
    private int FireBallPrepareBoss;

    private Queue<GameObject> _activeBombBoss;
    [SerializeField]
    private GameObject BombBoss;
    private int BombPrepareBoss;

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
        BombPrepareBoss = FireBallPrepareBoss = 5;
        _activeBomb = new Queue<GameObject>();
        _activeDarts = new Queue<GameObject>();
        _activeFireBall = new Queue<GameObject>();
        _activeBombBoss = new Queue<GameObject>();
        _activeFireBallBoss = new Queue<GameObject>();
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

    public void PrepareFireBallBoss()
    {
        for (int i = 0; i < FireBallPrepareBoss; i++)
        {
            GameObject fireballBoss = Instantiate(FireBallBoss, transform);
            fireballBoss.gameObject.SetActive(false);
            this._activeFireBallBoss.Enqueue(fireballBoss);
        }
    }
    public GameObject TakeFireBallBoss()
    {
        if (_activeFireBallBoss.Count <= 0)
        {
            PrepareFireBallBoss();
        }
        GameObject fireballBoss = this._activeFireBallBoss.Dequeue();
        fireballBoss.gameObject.SetActive(true);
        return fireballBoss;
    }
    public void ReturnFireBallBoss(GameObject fireballBoss)
    {
        this._activeFireBallBoss.Enqueue(fireballBoss);
        fireballBoss.SetActive(false);
    }

    public void PrepareBombBoss()
    {
        for (int i = 0; i < BombPrepareBoss; i++)
        {
            GameObject bombBoss = Instantiate(BombBoss, transform);
            bombBoss.gameObject.SetActive(false);
            _activeBombBoss.Enqueue(bombBoss);
        }
    }
    public GameObject TakeBombBoss()
    {
        if (_activeBombBoss.Count <= 0)
        {
            PrepareBombBoss();
        }
        GameObject bombBoss = this._activeBombBoss.Dequeue();
        bombBoss.gameObject.SetActive(true);
        return bombBoss;
    }
    public void ReturnBombBoss(GameObject bombBoss)
    {
        BombBoss b = bombBoss.GetComponent<BombBoss>();
        b.timeExplode = 1f;
        this._activeBombBoss.Enqueue(bombBoss);
        bombBoss.SetActive(false);
    }
}
