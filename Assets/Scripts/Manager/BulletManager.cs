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

    private Queue<GameObject> _activeSentoryu1;
    [SerializeField]
    private GameObject Sentoryu1;
    private int Sentoryu1Prepare;

    private Queue<GameObject> _activeSentoryu2;
    [SerializeField]
    private GameObject Sentoryu2;
    private int Sentoryu2Prepare;

    private Queue<GameObject> _activeSentoryu3;
    [SerializeField]
    private GameObject Sentoryu3;
    private int Sentoryu3Prepare;

    private Queue<GameObject> _activeHoaDon;
    [SerializeField]
    private GameObject HoaDon;
    private int HoaDonPrepare;

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
        Sentoryu1Prepare = Sentoryu2Prepare = Sentoryu3Prepare = 1;
        HoaDonPrepare = 1;
        _activeBomb = new Queue<GameObject>();
        _activeDarts = new Queue<GameObject>();
        _activeFireBall = new Queue<GameObject>();
        _activeBombBoss = new Queue<GameObject>();
        _activeFireBallBoss = new Queue<GameObject>();
        _activeSentoryu1 = new Queue<GameObject>();
        _activeSentoryu2 = new Queue<GameObject>();
        _activeSentoryu3 = new Queue<GameObject>();
        _activeHoaDon = new Queue<GameObject>();
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

    public void PrepareSentoryu1()
    {
        for(int i = 0; i< Sentoryu1Prepare; i++)
        {
            GameObject str1 = Instantiate(Sentoryu1, transform);
            str1.gameObject.SetActive(false);
            _activeSentoryu1.Enqueue(str1);
        }
    }

    public GameObject TakeSentoryu1()
    {
        if(_activeSentoryu1.Count <= 0)
        {
            PrepareSentoryu1();
        }
        GameObject str1 = this._activeSentoryu1.Dequeue();
        str1.gameObject.SetActive(true);
        return str1;
    }

    public void ReturnSentoryu1(GameObject str1)
    {
        this._activeSentoryu1.Enqueue(str1);
        str1.gameObject.SetActive(false);
    }

    public void PrepareSentoryu2()
    {
        for (int i = 0; i < Sentoryu2Prepare; i++)
        {
            GameObject str2 = Instantiate(Sentoryu2, transform);
            str2.gameObject.SetActive(false);
            _activeSentoryu2.Enqueue(str2);
        }
    }

    public GameObject TakeSentoryu2()
    {
        if (_activeSentoryu2.Count <= 0)
        {
            PrepareSentoryu2();
        }
        GameObject str2 = this._activeSentoryu2.Dequeue();
        str2.gameObject.SetActive(true);
        return str2;
    }

    public void ReturnSentoryu2(GameObject str2)
    {
        this._activeSentoryu2.Enqueue(str2);
        str2.gameObject.SetActive(false);
    }

    public void PrepareSentoryu3()
    {
        for (int i = 0; i < Sentoryu3Prepare; i++)
        {
            GameObject str3 = Instantiate(Sentoryu3, transform);
            str3.gameObject.SetActive(false);
            _activeSentoryu3.Enqueue(str3);
        }
    }

    public GameObject TakeSentoryu3()
    {
        if (_activeSentoryu3.Count <= 0)
        {
            PrepareSentoryu3();
        }
        GameObject str3 = this._activeSentoryu3.Dequeue();
        str3.gameObject.SetActive(true);
        return str3;
    }

    public void ReturnSentoryu3(GameObject str3)
    {
        this._activeSentoryu3.Enqueue(str3);
        str3.gameObject.SetActive(false);
    }

    public void PrepareHoaDon()
    {
        for(int i = 0;i < HoaDonPrepare; i++)
        {
            GameObject hoadon = Instantiate(HoaDon, transform);
            hoadon.gameObject.SetActive(false);
            _activeHoaDon.Enqueue(hoadon);
        }
    }

    public GameObject TakeHoaDon()
    {
        if(_activeHoaDon.Count <= 0)
        {
            PrepareHoaDon();
        }
        GameObject hoadon = _activeHoaDon.Dequeue();
        hoadon.gameObject.SetActive(true);
        return hoadon;
    }

    public void ReturnHoaDon(GameObject hoadon)
    {
        this._activeHoaDon.Enqueue(hoadon);
        hoadon.gameObject.SetActive(false);
    }
}
