using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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

    public PlayerGameData GameData;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }
    void Start()
    {
        //LoadGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }
    //public void SaveGame()
    //{
    //    Debug.Log(GameData.Hp);
    //    SaveSystem.SaveData(GameData);
    //}
    //public void LoadGame()
    //{
    //    SaveSystem.LoadData();
    //}
    public void ButtonQuitGame()
    {
        SceneManager.LoadScene("Start Game");
    }
}
