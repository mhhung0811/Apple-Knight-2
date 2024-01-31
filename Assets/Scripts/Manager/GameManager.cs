using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    private int Coin;

    public Slider HP_Silder;
    public TextMeshProUGUI HP_Text;
    public TextMeshProUGUI Coin_Text;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }
    void Start()
    {
        Debug.Log("GameManager");
        // Set up Coin
        Coin = 0;
        Coin_Text.text = Coin.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        Debug.Log("gameover");
    }

    public void CollectCoin()
    {
        Coin++;
        Coin_Text.text = Coin.ToString();
    }
}
