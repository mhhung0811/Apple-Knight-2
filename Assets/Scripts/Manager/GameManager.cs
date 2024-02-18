using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    private bool _isPauseGame;

    public Slider HP_Silder;
    public TextMeshProUGUI HP_Text;
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
        _isPauseGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        Debug.Log("gameover");
        UIManager.Instance.GameOver(); 
    }

    public void CollectCoin()
    {
        Coin++;
    }
    public bool PauseGame()
    {
        if (_isPauseGame)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ButtonPause()
    {
        _isPauseGame = true;
        Time.timeScale = 0;
    }
    public void ButtonResume()
    {
        _isPauseGame = false;
        Time.timeScale = 1;
    }
    public void ReStartGame()
    {
        _isPauseGame = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Victory()
    {
        ButtonPause();
        UIManager.Instance.Victory();
    }
}
